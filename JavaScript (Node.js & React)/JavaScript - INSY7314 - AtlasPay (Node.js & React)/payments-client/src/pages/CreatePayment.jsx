// src/pages/CreatePayment.jsx
import React, { useEffect, useMemo, useState } from "react";
import api, { postWithCsrf } from "../services/api";

export default function CreatePayment() {
  const [beneficiaries, setBeneficiaries] = useState([]);
  const [amount, setAmount] = useState("");
  const [currency, setCurrency] = useState("USD");
  const [beneficiaryId, setBeneficiaryId] = useState("");
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");

  const styles = useMemo(
    () => ({
      wrap: { maxWidth: 600, margin: "20px auto", fontSize: 14 },
      card: {
        background: "#fff",
        color: "#222",
        border: "1px solid #e5e7eb",
        borderRadius: 10,
        padding: 16,
        boxShadow: "0 2px 8px rgba(0,0,0,0.05)",
      },
      h2: { margin: "0 0 12px", fontSize: 20 },
      label: { display: "block", margin: "8px 0 4px", fontWeight: 600 },
      input: {
        width: "100%",
        padding: "8px 10px",
        border: "1px solid #d1d5db",
        borderRadius: 8,
        fontSize: 14,
      },
      btn: {
        marginTop: 12,
        padding: "8px 12px",
        borderRadius: 10,
        fontSize: 14,
        background: "#111827",
        color: "white",
        border: "1px solid #111827",
        cursor: "pointer",
      },
      help: { fontSize: 12, color: "#6b7280", marginTop: 2 },
      tipIcon: {
        marginLeft: 6,
        cursor: "help",
        color: "#6b7280",
        fontWeight: 700,
      },
    }),
    []
  );

  useEffect(() => {
    api.get("/beneficiaries").then((res) => setBeneficiaries(res.data || []));
  }, []);

  async function handleSubmit(e) {
    e.preventDefault();
    setMessage("");
    setLoading(true);
    try {
      if (!beneficiaryId) {
        setMessage("Please choose a beneficiary.");
        setLoading(false);
        return;
      }
      const res = await postWithCsrf("/payments/create", {
        beneficiaryId,
        amount: Number(amount),
        currency,
      });
      setMessage("Payment submitted! Status: " + res.data.status);
      setAmount("");
      setCurrency("USD");
      setBeneficiaryId("");
    } catch (err) {
      setMessage(err?.response?.data?.error || "Payment failed");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div style={styles.wrap}>
      <div style={styles.card}>
        <h2 style={styles.h2}>Make a Payment</h2>
        {message && (
          <div
            style={{
              marginBottom: 10,
              fontSize: 13,
              color: message.includes("failed") ? "#b91c1c" : "#065f46",
            }}
          >
            {message}
          </div>
        )}
        <form onSubmit={handleSubmit}>
          <label style={styles.label}>
            Beneficiary
            <span
              style={styles.tipIcon}
              title="Choose who to pay. Staff-created beneficiaries for your account appear here."
            >
              ⓘ
            </span>
          </label>
          <select
            value={beneficiaryId}
            onChange={(e) => setBeneficiaryId(e.target.value)}
            required
            style={styles.input}
          >
            <option value="">Select a beneficiary…</option>
            {beneficiaries.map((b) => (
              <option key={b._id} value={b._id}>
                {b.name} — {b.bankName} — {b.beneficiaryAccountNumber}
              </option>
            ))}
          </select>
          <div style={styles.help}>
            Can’t see a beneficiary? Ask staff to create one for you.
          </div>

          <label style={styles.label}>
            Amount
            <span
              style={styles.tipIcon}
              title="Enter the amount to send (e.g., 125.50)."
            >
              ⓘ
            </span>
          </label>
          <input
            type="number"
            value={amount}
            onChange={(e) => setAmount(e.target.value)}
            min="0.01"
            step="0.01"
            required
            placeholder="e.g. 150.00"
            style={styles.input}
          />
          <div style={styles.help}>Use a dot for decimals.</div>

          <label style={styles.label}>
            Currency
            <span
              style={styles.tipIcon}
              title="Pick the currency for this payment."
            >
              ⓘ
            </span>
          </label>
          <select
            value={currency}
            onChange={(e) => setCurrency(e.target.value)}
            style={styles.input}
          >
            <option value="USD">USD</option>
            <option value="EUR">EUR</option>
            <option value="ZAR">ZAR</option>
          </select>

          <button type="submit" disabled={loading} style={styles.btn}>
            {loading ? "Submitting…" : "Submit Payment"}
          </button>
        </form>
      </div>
    </div>
  );
}
