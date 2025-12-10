// src/pages/StaffDashboard.jsx
import React, { useEffect, useMemo, useState } from "react";
import api, { postWithCsrf, putWithCsrf } from "../services/api";

// ---------- Validation helpers ----------
const OID_RE = /^[a-f\d]{24}$/i;
const DIGITS_6_30_RE = /^[0-9]{6,30}$/;
const BIC_RE = /^[A-Z0-9]{8}([A-Z0-9]{3})?$/;

function validateBeneficiary(form) {
  const errs = {};

  // OwnerId
  if (!form.ownerId || !OID_RE.test(String(form.ownerId).trim())) {
    errs.ownerId =
      "Owner ID is required and must be a valid MongoDB ObjectId (24 hex characters).";
  }

  // Name
  const name = (form.name || "").trim();
  if (name.length < 2 || name.length > 80) {
    errs.name = "Beneficiary name must be between 2 and 80 characters.";
  }

  // Bank name
  const bank = (form.bankName || "").trim();
  if (bank.length < 2 || bank.length > 80) {
    errs.bankName = "Bank name must be between 2 and 80 characters.";
  }

  // Account number
  const acc = String(form.beneficiaryAccountNumber || "").trim();
  if (!DIGITS_6_30_RE.test(acc)) {
    errs.beneficiaryAccountNumber =
      "Account number must be digits only (6–30 characters).";
  }

  // SWIFT (optional)
  const swift = (form.swiftCode || "").trim();
  if (swift && !BIC_RE.test(swift)) {
    errs.swiftCode =
      "SWIFT/BIC must be 8 or 11 uppercase letters/numbers (e.g., BANKZAJJXXX).";
  }

  return errs;
}

export default function StaffDashboard() {
  const [beneficiaries, setBeneficiaries] = useState([]);
  const [payments, setPayments] = useState([]);
  const [loading, setLoading] = useState(false);

  // UI messages
  const [serverError, setServerError] = useState("");
  const [serverOk, setServerOk] = useState("");

  // Client-side validation errors
  const [errors, setErrors] = useState({});

  const [beneficiaryForm, setBeneficiaryForm] = useState({
    ownerId: "",
    ownerChoiceId: "",
    name: "",
    beneficiaryAccountNumber: "",
    bankName: "",
    swiftCode: "",
  });

  // Derive a list of customers from payments for the dropdown convenience
  const customersFromPayments = useMemo(() => {
    const map = new Map();
    for (const p of payments) {
      const payer = p?.payerId;
      if (payer && payer._id && payer.email) {
        map.set(payer._id, { id: payer._id, email: payer.email });
      }
    }
    return Array.from(map.values()).sort((a, b) =>
      (a.email || "").localeCompare(b.email || "")
    );
  }, [payments]);

  async function loadAll() {
    setLoading(true);
    setServerError("");
    try {
      const [bRes, pRes] = await Promise.all([
        api.get("/beneficiaries"),
        api.get("/payments/all"),
      ]);
      setBeneficiaries(bRes.data || []);
      setPayments(pRes.data || []);
    } catch (err) {
      setServerError(err?.response?.data?.error || "Failed to load dashboard");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadAll();
  }, []);

  // When user selects an email, auto-fill ownerId
  useEffect(() => {
    const selected = customersFromPayments.find(
      (c) => c.id === beneficiaryForm.ownerChoiceId
    );
    if (selected) {
      setBeneficiaryForm((f) => ({ ...f, ownerId: selected.id }));
      setErrors((e) => {
        const next = { ...e };
        delete next.ownerId;
        return next;
      });
    }
  }, [beneficiaryForm.ownerChoiceId, customersFromPayments]);

  // Handle change + live validation
  function onChange(field, value) {
    const next = { ...beneficiaryForm, [field]: value };
    setBeneficiaryForm(next);

    // live-validate this field
    const fieldErrs = validateBeneficiary(next);
    setErrors(fieldErrs);
  }

  async function handleCreateBeneficiary(e) {
    e.preventDefault();
    setServerError("");
    setServerOk("");

    // final validation pass
    const fieldErrs = validateBeneficiary(beneficiaryForm);
    setErrors(fieldErrs);
    if (Object.keys(fieldErrs).length > 0) {
      setServerError("Please fix the highlighted fields below and try again.");
      return;
    }

    try {
      const payload = {
        ownerId: beneficiaryForm.ownerId,
        name: beneficiaryForm.name.trim(),
        beneficiaryAccountNumber: String(
          beneficiaryForm.beneficiaryAccountNumber
        ).trim(),
        bankName: beneficiaryForm.bankName.trim(),
        swiftCode: beneficiaryForm.swiftCode.trim() || undefined,
      };

      await postWithCsrf("/beneficiaries", payload);
      setServerOk("✅ Beneficiary created successfully.");
      setBeneficiaryForm({
        ownerId: "",
        ownerChoiceId: "",
        name: "",
        beneficiaryAccountNumber: "",
        bankName: "",
        swiftCode: "",
      });
      setErrors({});
      await loadAll();
    } catch (err) {
      const apiErr =
        err?.response?.data?.error ||
        err?.response?.data?.details?.[0]?.message ||
        "Failed to create beneficiary";
      setServerError(apiErr);
    }
  }

  const isInvalid = (key) => Boolean(errors[key]);

  const invalidStyle = {
    borderColor: "#5a1f28",
    boxShadow: "0 0 0 4px rgba(248,113,113,0.15)",
  };

  return (
    <>
      <div className="card">
        <h2>Staff Dashboard</h2>

        {(serverError || serverOk) && (
          <div
            className="small"
            style={{
              marginTop: 12,
              padding: "10px 12px",
              borderRadius: 10,
              border: `1px solid ${
                serverError ? "#5a1f28" : "rgba(110,168,254,.5)"
              }`,
              background: serverError ? "#271317" : "rgba(110,168,254,.08)",
              color: serverError ? "#ffd7db" : "#dbe9ff",
            }}
          >
            {serverError || serverOk}
            {/* Error list summary (friendly) */}
            {serverError && Object.keys(errors).length > 0 && (
              <ul style={{ marginTop: 8, marginBottom: 0, paddingLeft: 18 }}>
                {Object.entries(errors).map(([k, v]) => (
                  <li key={k}>{v}</li>
                ))}
              </ul>
            )}
          </div>
        )}
      </div>

      <div className="card">
        <h3>Create Beneficiary</h3>
        <form onSubmit={handleCreateBeneficiary} noValidate>
          <div className="grid-2">
            {/* Owner chooser */}
            <div>
              <label className="label">
                Owner (Customer)
                <span
                  className="tooltip"
                  data-tip="Pick a customer by email (auto-fills Owner ID)."
                  title="Pick a customer by email (auto-fills Owner ID)."
                >
                  ⓘ
                </span>
              </label>
              <select
                className="select"
                value={beneficiaryForm.ownerChoiceId}
                onChange={(e) => onChange("ownerChoiceId", e.target.value)}
              >
                <option value="">Select by customer email…</option>
                {customersFromPayments.map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.email}
                  </option>
                ))}
              </select>
              <div className="help">
                Don’t see the customer? Paste their MongoDB <code>_id</code>{" "}
                below.
              </div>
            </div>

            {/* Owner ID */}
            <div>
              <label className="label">
                Owner ID (required)
                <span
                  className="tooltip"
                  data-tip="Auto-filled from dropdown. You can paste an _id to override."
                  title="Auto-filled from dropdown. You can paste an _id to override."
                >
                  ⓘ
                </span>
              </label>
              <input
                className="input"
                placeholder="e.g. 68e7c21bc241f5f75b2a2bf9"
                value={beneficiaryForm.ownerId}
                onChange={(e) => onChange("ownerId", e.target.value)}
                style={isInvalid("ownerId") ? invalidStyle : undefined}
                aria-invalid={isInvalid("ownerId")}
                aria-describedby={isInvalid("ownerId") ? "err-ownerId" : undefined}
                required
              />
              {isInvalid("ownerId") && (
                <div id="err-ownerId" className="help" style={{ color: "#ffd7db" }}>
                  {errors.ownerId}
                </div>
              )}
            </div>
          </div>

          <div className="grid-3" style={{ marginTop: 8 }}>
            {/* Name */}
            <div>
              <label className="label">
                Beneficiary Name
                <span
                  className="tooltip"
                  data-tip="Company or person (e.g., ACME Ltd)."
                  title="Company or person (e.g., ACME Ltd)."
                >
                  ⓘ
                </span>
              </label>
              <input
                className="input"
                placeholder="e.g. ACME Ltd"
                value={beneficiaryForm.name}
                onChange={(e) => onChange("name", e.target.value)}
                style={isInvalid("name") ? invalidStyle : undefined}
                aria-invalid={isInvalid("name")}
                aria-describedby={isInvalid("name") ? "err-name" : undefined}
                required
              />
              {isInvalid("name") && (
                <div id="err-name" className="help" style={{ color: "#ffd7db" }}>
                  {errors.name}
                </div>
              )}
            </div>

            {/* Bank */}
            <div>
              <label className="label">
                Bank Name
                <span
                  className="tooltip"
                  data-tip="Bank or provider (e.g., Local Bank)."
                  title="Bank or provider (e.g., Local Bank)."
                >
                  ⓘ
                </span>
              </label>
              <input
                className="input"
                placeholder="e.g. Local Bank"
                value={beneficiaryForm.bankName}
                onChange={(e) => onChange("bankName", e.target.value)}
                style={isInvalid("bankName") ? invalidStyle : undefined}
                aria-invalid={isInvalid("bankName")}
                aria-describedby={isInvalid("bankName") ? "err-bank" : undefined}
                required
              />
              {isInvalid("bankName") && (
                <div id="err-bank" className="help" style={{ color: "#ffd7db" }}>
                  {errors.bankName}
                </div>
              )}
            </div>

            {/* Account */}
            <div>
              <label className="label">
                Account Number
                <span
                  className="tooltip"
                  data-tip="Digits only, 6–30 characters."
                  title="Digits only, 6–30 characters."
                >
                  ⓘ
                </span>
              </label>
              <input
                className="input"
                placeholder="e.g. 12345678"
                value={beneficiaryForm.beneficiaryAccountNumber}
                onChange={(e) =>
                  onChange("beneficiaryAccountNumber", e.target.value)
                }
                style={
                  isInvalid("beneficiaryAccountNumber") ? invalidStyle : undefined
                }
                aria-invalid={isInvalid("beneficiaryAccountNumber")}
                aria-describedby={
                  isInvalid("beneficiaryAccountNumber") ? "err-acc" : undefined
                }
                required
                inputMode="numeric"
                pattern="[0-9]*"
              />
              {isInvalid("beneficiaryAccountNumber") && (
                <div id="err-acc" className="help" style={{ color: "#ffd7db" }}>
                  {errors.beneficiaryAccountNumber}
                </div>
              )}
            </div>
          </div>

          <div className="grid-2" style={{ marginTop: 8 }}>
            {/* SWIFT */}
            <div>
              <label className="label">
                SWIFT (optional)
                <span
                  className="tooltip"
                  data-tip="8 or 11 uppercase letters/numbers (e.g., BANKZAJJXXX)."
                  title="8 or 11 uppercase letters/numbers (e.g., BANKZAJJXXX)."
                >
                  ⓘ
                </span>
              </label>
              <input
                className="input"
                placeholder="e.g. BANKZAJJXXX"
                value={beneficiaryForm.swiftCode}
                onChange={(e) => onChange("swiftCode", e.target.value.toUpperCase())}
                style={isInvalid("swiftCode") ? invalidStyle : undefined}
                aria-invalid={isInvalid("swiftCode")}
                aria-describedby={isInvalid("swiftCode") ? "err-swift" : undefined}
              />
              {isInvalid("swiftCode") && (
                <div id="err-swift" className="help" style={{ color: "#ffd7db" }}>
                  {errors.swiftCode}
                </div>
              )}
            </div>

            {/* Actions */}
            <div style={{ display: "flex", alignItems: "flex-end", gap: 10 }}>
              <button
                type="submit"
                className="btn btn-primary"
                disabled={loading || Object.keys(errors).length > 0}
                title={
                  Object.keys(errors).length > 0
                    ? "Fix validation errors to enable"
                    : "Create Beneficiary"
                }
              >
                Create Beneficiary
              </button>
              <button
                type="button"
                className="btn btn-ghost"
                onClick={() => {
                  setBeneficiaryForm({
                    ownerId: "",
                    ownerChoiceId: "",
                    name: "",
                    beneficiaryAccountNumber: "",
                    bankName: "",
                    swiftCode: "",
                  });
                  setErrors({});
                  setServerError("");
                  setServerOk("");
                }}
              >
                Clear
              </button>
            </div>
          </div>
        </form>
      </div>

      {/* Payments table */}
      <div className="card">
        <h3>All Payments</h3>
        {loading ? (
          <div className="help">Loading payments…</div>
        ) : payments.length === 0 ? (
          <div className="help">No payments yet.</div>
        ) : (
          <div className="table-wrap">
            <table className="table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Payer</th>
                  <th>Beneficiary</th>
                  <th>Amount</th>
                  <th>Currency</th>
                  <th>Provider</th>
                  <th>Status</th>
                  <th>Created</th>
                  <th>Action</th>
                </tr>
              </thead>
              <tbody>
                {payments.map((p) => {
                  const cls =
                    p.status === "verified"
                      ? "badge verified"
                      : p.status === "declined"
                      ? "badge declined"
                      : "badge pending";
                  return (
                    <tr key={p._id}>
                      <td title={p._id}>
                        {p._id.slice(0, 6)}…{p._id.slice(-4)}
                      </td>
                      <td>{p?.payerId?.email || "—"}</td>
                      <td>{p?.beneficiaryId?.name || "—"}</td>
                      <td>{p.amount}</td>
                      <td>{p.currency}</td>
                      <td>{p.provider}</td>
                      <td>
                        <span className={cls}>{p.status}</span>
                      </td>
                      <td>{new Date(p.createdAt).toLocaleString()}</td>
                      <td>
                        {p.status === "pending" ? (
                          <button
                            onClick={async () => {
                              try {
                                await putWithCsrf(`/payments/verify/${p._id}`, {
                                  status: "verified",
                                });
                                setServerOk("✅ Payment verified.");
                                await loadAll();
                              } catch (err) {
                                setServerError(
                                  err?.response?.data?.error ||
                                    "Failed to verify payment"
                                );
                              }
                            }}
                            className="btn btn-ghost small"
                          >
                            Verify
                          </button>
                        ) : (
                          <span className="muted small">—</span>
                        )}
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        )}
        <div style={{ marginTop: 10 }}>
          <button onClick={loadAll} className="btn btn-ghost small">
            Refresh
          </button>
        </div>
      </div>

      {/* Beneficiaries table */}
      <div className="card">
        <h3>Beneficiaries</h3>
        {beneficiaries.length === 0 ? (
          <div className="help">No beneficiaries.</div>
        ) : (
          <div className="table-wrap">
            <table className="table">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Bank</th>
                  <th>Account #</th>
                  <th>SWIFT</th>
                  <th>Owner ID</th>
                  <th>Created</th>
                </tr>
              </thead>
              <tbody>
                {beneficiaries.map((b) => (
                  <tr key={b._id}>
                    <td>{b.name}</td>
                    <td>{b.bankName}</td>
                    <td>{b.beneficiaryAccountNumber}</td>
                    <td>{b.swiftCode || "—"}</td>
                    <td title={b.ownerId}>
                      {String(b.ownerId).slice(0, 6)}…
                      {String(b.ownerId).slice(-4)}
                    </td>
                    <td>{new Date(b.createdAt).toLocaleString()}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </>
  );
}
