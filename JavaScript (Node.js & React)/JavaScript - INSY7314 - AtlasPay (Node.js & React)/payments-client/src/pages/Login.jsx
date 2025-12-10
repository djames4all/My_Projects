// src/pages/Login.jsx
import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { postWithCsrf } from "../services/api";

export default function Login({ onAuth }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPw, setShowPw] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  async function handleSubmit(e) {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const res = await postWithCsrf("/auth/login", { email, password });
      const user = res.data.user || res.data;
      const role = user?.role;
      if (!role) throw new Error("Login succeeded but no role returned");

      onAuth?.(role);
      navigate(role === "staff" ? "/staff" : "/customer");
    } catch (err) {
      setError(err.response?.data?.error || "Login failed");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div style={{ display: "grid", placeItems: "center" }}>
      <div className="card" style={{ width: "min(560px, 92vw)", padding: 24 }}>
        <h2 style={{ marginBottom: 6 }}>Sign in</h2>
        <p className="muted small" style={{ marginTop: 0 }}>
          Welcome back. Enter your details to continue.
        </p>

        {error && (
          <div
            className="small"
            style={{
              marginTop: 12,
              marginBottom: 12,
              padding: "10px 12px",
              borderRadius: 10,
              border: "1px solid #5a1f28",
              background: "#271317",
              color: "#ffd7db",
            }}
          >
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} style={{ marginTop: 8 }}>
          <label className="label">
            Email
            <span
              className="tooltip"
              title="Use the email you registered with."
              data-tip="Use the email you registered with."
            >
              ⓘ
            </span>
          </label>
          <input
            className="input"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            type="email"
            required
            placeholder="you@example.com"
            autoComplete="email"
          />

          <div className="grid-2" style={{ marginTop: 10 }}>
            <div>
              <label className="label">
                Password
                <span
                  className="tooltip"
                  title="Password is case-sensitive."
                  data-tip="Password is case-sensitive."
                >
                  ⓘ
                </span>
              </label>
              <input
                className="input"
                id="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                type={showPw ? "text" : "password"}
                required
                placeholder="Your password"
                autoComplete="current-password"
              />
            </div>

            <div style={{ display: "flex", alignItems: "flex-end" }}>
              <button
                type="button"
                className="btn btn-ghost"
                onClick={() => setShowPw((s) => !s)}
                aria-pressed={showPw}
                title={showPw ? "Hide password" : "Show password"}
              >
                {showPw ? "Hide" : "Show"} password
              </button>
            </div>
          </div>

          <div style={{ display: "flex", gap: 10, marginTop: 16 }}>
            <button type="submit" className="btn btn-primary" disabled={loading}>
              {loading ? "Signing in…" : "Sign in"}
            </button>
            <Link to="/register" className="btn btn-ghost">
              Create an account
            </Link>
          </div>

          <p className="muted small" style={{ marginTop: 10 }}>
            Tip: Staff account — <code>admin@example.com</code>
          </p>
        </form>
      </div>
    </div>
  );
}
