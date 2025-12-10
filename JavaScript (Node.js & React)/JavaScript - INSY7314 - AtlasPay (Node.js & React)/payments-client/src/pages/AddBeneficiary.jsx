// src/pages/AddBeneficiary.jsx
import React, { useState } from "react";
import { postWithCsrf } from "../services/api";

export default function AddBeneficiary({ onDone }) {
  const [form, setForm] = useState({ name: "", beneficiaryAccountNumber: "", bankName: "", swiftCode: "" });
  const [err, setErr] = useState("");
  async function submit(e) {
    e.preventDefault();
    setErr("");
    try {
      await postWithCsrf("/beneficiaries", form);
      if (onDone) onDone();
    } catch (e) { setErr(e?.response?.data?.error || "Create failed"); }
  }
  return (
    <form onSubmit={submit}>
      {err && <div style={{ color: "crimson" }}>{err}</div>}
      <input placeholder="Name" value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} required />
      <input placeholder="Account" value={form.beneficiaryAccountNumber} onChange={e => setForm({ ...form, beneficiaryAccountNumber: e.target.value })} required />
      <input placeholder="Bank" value={form.bankName} onChange={e => setForm({ ...form, bankName: e.target.value })} />
      <input placeholder="SWIFT" value={form.swiftCode} onChange={e => setForm({ ...form, swiftCode: e.target.value })} />
      <button type="submit">Add Beneficiary</button>
    </form>
  );
}
