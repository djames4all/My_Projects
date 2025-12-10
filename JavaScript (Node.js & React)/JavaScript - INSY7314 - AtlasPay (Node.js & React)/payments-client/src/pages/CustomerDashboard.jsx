// src/pages/CustomerDashboard.jsx
import React, { useEffect, useState } from "react";
import api, { postWithCsrf } from "../services/api";

const CURRENCIES = [
  "USD","EUR","GBP","ZAR","AUD","CAD","NZD","JPY","CNY","INR",
  "CHF","SEK","NOK","DKK","AED","SAR","HKD","SGD"
];

export default function CustomerDashboard() {
  const [beneficiaries, setBeneficiaries] = useState([]);
  const [payments, setPayments] = useState([]);
  const [form, setForm] = useState({
    beneficiaryId: "",
    amount: "",
    currency: "USD",
  });
  const [busy, setBusy] = useState(false);
  const [msg, setMsg] = useState("");
  const [err, setErr] = useState("");

  async function load() {
    setErr(""); setMsg("");
    try {
      const [b, p] = await Promise.all([
        api.get("/beneficiaries"),
        api.get("/payments/my-payments"),
      ]);
      setBeneficiaries(b.data || []);
      setPayments(p.data || []);
    } catch (e) {
      setErr(e?.response?.data?.error || "Failed to load data");
    }
  }

  useEffect(() => { load(); }, []);

  function onChange(field, value) {
    setForm((f) => ({ ...f, [field]: value }));
  }

  async function submitPayment(e) {
    e.preventDefault();
    setErr(""); setMsg("");
    const amt = Number(form.amount);
    if (!form.beneficiaryId) return setErr("Please select a beneficiary.");
    if (!amt || amt <= 0) return setErr("Amount must be greater than 0.");

    setBusy(true);
    try {
      await postWithCsrf("/payments/create", {
        beneficiaryId: form.beneficiaryId,
        amount: amt,
        currency: form.currency,
      });
      setMsg("✅ Payment submitted.");
      setForm({ beneficiaryId: "", amount: "", currency: form.currency });
      await load();
    } catch (e) {
      setErr(e?.response?.data?.error || "Failed to submit payment");
    } finally {
      setBusy(false);
    }
  }

  return (
    <>
      <div className="card">
        <h2>Create Payment</h2>

        {(err || msg) && (
          <div
            className="small"
            style={{
              marginTop: 12,
              padding: "10px 12px",
              borderRadius: 10,
              border: `1px solid ${err ? "#5a1f28" : "rgba(110,168,254,.5)"}`,
              background: err ? "#271317" : "rgba(110,168,254,.08)",
              color: err ? "#ffd7db" : "#dbe9ff",
            }}
          >
            {err || msg}
          </div>
        )}

        <form onSubmit={submitPayment} style={{ marginTop: 8 }}>
          <div className="grid-2">
            <div>
              <label className="label">
                Beneficiary <span className="tooltip" title="Choose who to pay">ⓘ</span>
              </label>
              <select
                className="select"
                value={form.beneficiaryId}
                onChange={(e) => onChange("beneficiaryId", e.target.value)}
                required
              >
                <option value="">Select a beneficiary…</option>
                {beneficiaries.map((b) => (
                  <option key={b._id} value={b._id}>
                    {b.name} — {b.bankName} ({b.beneficiaryAccountNumber})
                  </option>
                ))}
              </select>
              <div className="help">Can’t see a beneficiary? Ask staff to create one for you.</div>
            </div>

            <div>
              <label className="label">
                Amount <span className="tooltip" title="Use a dot for decimals">ⓘ</span>
              </label>
              <input
                className="input"
                placeholder="e.g. 150.00"
                value={form.amount}
                onChange={(e) => onChange("amount", e.target.value)}
                inputMode="decimal"
                required
              />
            </div>
          </div>

          <div className="grid-2" style={{ marginTop: 8 }}>
            <div>
              <label className="label">
                Currency <span className="tooltip" title="Supported settlement currencies">ⓘ</span>
              </label>
              <select
                className="select"
                value={form.currency}
                onChange={(e) => onChange("currency", e.target.value)}
              >
                {CURRENCIES.map((c) => (
                  <option key={c} value={c}>{c}</option>
                ))}
              </select>
              <div className="help">If the currency you want isn’t here, ask staff to add it to the allow-list.</div>
            </div>

            <div style={{ display: "flex", alignItems: "flex-end", gap: 10 }}>
              <button className="btn btn-primary" type="submit" disabled={busy}>
                {busy ? "Submitting…" : "Submit Payment"}
              </button>
              <button
                type="button"
                className="btn btn-ghost"
                onClick={() => setForm({ beneficiaryId: "", amount: "", currency: "USD" })}
              >
                Clear
              </button>
            </div>
          </div>
        </form>
      </div>

      <div className="card">
        <h3>Your Beneficiaries</h3>
        {beneficiaries.length === 0 ? (
          <div className="help">None yet. Staff can add one for you.</div>
        ) : (
          <div className="table-wrap">
            <table className="table">
              <thead>
                <tr>
                  <th>Name</th><th>Bank</th><th>Account #</th><th>SWIFT</th>
                </tr>
              </thead>
              <tbody>
                {beneficiaries.map((b) => (
                  <tr key={b._id}>
                    <td>{b.name}</td>
                    <td>{b.bankName}</td>
                    <td>{b.beneficiaryAccountNumber}</td>
                    <td>{b.swiftCode || "—"}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      <div className="card">
        <h3>Past Transactions</h3>
        {payments.length === 0 ? (
          <div className="help">No payments yet.</div>
        ) : (
          <div className="table-wrap">
            <table className="table">
              <thead>
                <tr>
                  <th>Beneficiary</th><th>Amount</th><th>Currency</th><th>Status</th><th>Created</th>
                </tr>
              </thead>
              <tbody>
                {payments.map((p) => (
                  <tr key={p._id}>
                    <td>{p?.beneficiaryId?.name || "—"}</td>
                    <td>{p.amount}</td>
                    <td>{p.currency}</td>
                    <td><span className={`badge ${p.status}`}>{p.status}</span></td>
                    <td>{new Date(p.createdAt).toLocaleString()}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
        <div style={{ marginTop: 10 }}>
          <button className="btn btn-ghost small" onClick={load}>Refresh</button>
        </div>
      </div>
    </>
  );
}
