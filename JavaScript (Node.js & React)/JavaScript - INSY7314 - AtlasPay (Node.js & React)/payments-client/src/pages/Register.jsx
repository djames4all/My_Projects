// src/pages/Register.jsx
import React, { useMemo, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { postWithCsrf } from "../services/api";

export default function Register({ onAuth }) {
  const [form, setForm] = useState({
    fullName: "",
    email: "",
    password: "",
    accountNumber: "",
    idNumber: "",
  });
  const [showPw, setShowPw] = useState(false);
  const [error, setError] = useState("");
  const [ok, setOk] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  function onChange(e) {
    setForm((f) => ({ ...f, [e.target.name]: e.target.value }));
  }

  const pwScore = useMemo(() => {
    const p = form.password || "";
    let s = 0;
    if (p.length >= 8) s++;
    if (/[A-Z]/.test(p)) s++;
    if (/[a-z]/.test(p)) s++;
    if (/\d/.test(p)) s++;
    if (/[^A-Za-z0-9]/.test(p)) s++;
    return Math.min(s, 4); // 0..4
  }, [form.password]);

  async function handleSubmit(e) {
    e.preventDefault();
    setError(""); setOk("");
    setLoading(true);
    try {
      const res = await postWithCsrf("/auth/register", form);
      const user = res.data?.user || res.data;
      const role = user?.role;
      if (!role) throw new Error("No role returned");
      setOk("Account created!");
      onAuth?.(role);
      navigate(role === "staff" ? "/staff" : "/customer");
    } catch (err) {
      setError(
        err?.response?.data?.error ||
          err?.message ||
          "Registration failed - check inputs"
      );
    } finally {
      setLoading(false);
    }
  }

  // small inline styles just for the password suffix button
  const pwWrap = {
    position: "relative",
    display: "grid",
  };
  const pwToggle = {
    position: "absolute",
    right: 6,
    top: 6,
    height: 32,
    padding: "0 10px",
    fontSize: 12,
    borderRadius: 8,
    border: "1px solid var(--border)",
    background: "transparent",
    color: "var(--text)",
    cursor: "pointer",
  };

  const strengthStyle = (score) => ({
    borderColor: score >= 3 ? "#0f5132" : score >= 2 ? "#3b4252" : "#5b1b1b",
    color:      score >= 3 ? "#a7f3d0" : score >= 2 ? "#e5e7eb" : "#fecaca",
    background: score >= 3 ? "#0b1b16" : score >= 2 ? "#0a0f16" : "#1a0d0d",
  });

  return (
    <div style={{ display: "grid", placeItems: "center" }}>
      <div className="card" style={{ width: "min(720px, 94vw)", padding: 24 }}>
        <h2 style={{ marginBottom: 6 }}>Register</h2>
        <p className="muted small" style={{ marginTop: 0 }}>
          Create your account to start sending payments.
        </p>

        {(error || ok) && (
          <div
            className="small"
            style={{
              marginTop: 12,
              marginBottom: 12,
              padding: "10px 12px",
              borderRadius: 10,
              border: `1px solid ${error ? "#5a1f28" : "rgba(110,168,254,.5)"}`,
              background: error ? "#271317" : "rgba(110,168,254,.08)",
              color: error ? "#ffd7db" : "#dbe9ff",
            }}
          >
            {error || ok}
          </div>
        )}

        <form onSubmit={handleSubmit}>
          {/* Row 1: Name / Email */}
          <div className="grid-2">
            <div>
              <label className="label">
                Full Name
                <span className="tooltip" title="Your legal name (for KYC)." data-tip="Your legal name (for KYC).">ⓘ</span>
              </label>
              <input
                name="fullName"
                className="input"
                placeholder="e.g. Ayush Harduth"
                value={form.fullName}
                onChange={onChange}
                required
                autoComplete="name"
              />
            </div>
            <div>
              <label className="label">
                Email
                <span className="tooltip" title="We’ll send alerts here." data-tip="We’ll send alerts here.">ⓘ</span>
              </label>
              <input
                name="email"
                className="input"
                type="email"
                placeholder="you@example.com"
                value={form.email}
                onChange={onChange}
                required
                autoComplete="email"
              />
            </div>
          </div>

          {/* Row 2: Password (with inline toggle) / Account Number */}
          <div className="grid-2" style={{ marginTop: 8 }}>
            <div>
              <label className="label">
                Password
                <span className="tooltip" title="Min 8 chars, mix recommended." data-tip="Min 8 chars, mix recommended.">ⓘ</span>
              </label>
              <div style={pwWrap}>
                <input
                  name="password"
                  className="input"
                  type={showPw ? "text" : "password"}
                  placeholder="••••••••"
                  value={form.password}
                  onChange={onChange}
                  required
                  autoComplete="new-password"
                  minLength={8}
                />
                <button
                  type="button"
                  onClick={() => setShowPw((s) => !s)}
                  style={pwToggle}
                  aria-pressed={showPw}
                  title={showPw ? "Hide password" : "Show password"}
                >
                  {showPw ? "Hide" : "Show"}
                </button>
              </div>
              <div className="help" style={{ marginTop: 6 }}>
                Strength:&nbsp;
                <span className="badge" style={strengthStyle(pwScore)}>
                  {["Very weak", "Weak", "Okay", "Good", "Strong"][pwScore]}
                </span>
              </div>
            </div>

            <div>
              <label className="label">
                Account Number
                <span className="tooltip" title="Your receiving account with us." data-tip="Your receiving account with us.">ⓘ</span>
              </label>
              <input
                name="accountNumber"
                className="input"
                placeholder="e.g. 10000001"
                value={form.accountNumber}
                onChange={onChange}
                required
                inputMode="numeric"
                pattern="[0-9]+"
              />
            </div>
          </div>

          {/* Row 3: ID Number (full width) */}
          <div style={{ marginTop: 8 }}>
            <label className="label">
              ID Number
              <span className="tooltip" title="National ID (for KYC verification)." data-tip="National ID (for KYC verification).">ⓘ</span>
            </label>
            <input
              name="idNumber"
              className="input"
              placeholder="e.g. 8001010000000"
              value={form.idNumber}
              onChange={onChange}
              required
              inputMode="numeric"
              pattern="[0-9]+"
            />
          </div>

          {/* Actions */}
          <div style={{ display: "flex", gap: 10, marginTop: 16 }}>
            <button type="submit" className="btn btn-primary" disabled={loading}>
              {loading ? "Registering…" : "Register"}
            </button>
            <Link to="/login" className="btn btn-ghost">
              I already have an account
            </Link>
          </div>
        </form>
      </div>
    </div>
  );
}
