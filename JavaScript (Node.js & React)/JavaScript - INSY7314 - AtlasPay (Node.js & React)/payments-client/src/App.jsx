// src/App.jsx
import React, { useState, useEffect } from "react";
import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import Login from "./pages/Login";
import Register from "./pages/Register";
import CustomerDashboard from "./pages/CustomerDashboard";
import StaffDashboard from "./pages/StaffDashboard";
import PrivateRoute from "./components/PrivateRoute";
import api, { fetchCsrfToken } from "./services/api";
import Logo from "./components/Logo";

export default function App() {
  const [role, setRole] = useState(null);
  const [loading, setLoading] = useState(true);
  const [bootError, setBootError] = useState("");

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        const res = await api.get("/auth/me").catch(() => null);
        const user = res?.data?.user;
        if (mounted && user?.role) {
          localStorage.setItem("role", user.role);
          setRole(user.role);
        }
      } catch (err) {
        console.warn("Boot /auth/me failed", err);
        setBootError(err?.message || "Boot error");
        localStorage.removeItem("role");
        setRole(null);
      } finally {
        if (mounted) setLoading(false);
      }
    })();
    return () => (mounted = false);
  }, []);

  function handleSetRole(r) {
    try {
      if (r) {
        localStorage.setItem("role", r);
        setRole(r);
      } else {
        localStorage.removeItem("role");
        setRole(null);
      }
    } catch {}
  }

  async function handleLogout() {
    try {
      const csrf = await fetchCsrfToken();
      await api.post("/auth/logout", {}, { headers: { "X-CSRF-Token": csrf } });
    } catch {}
    localStorage.removeItem("role");
    setRole(null);
  }

  return (
    <BrowserRouter>
      {/* NAVBAR */}
      <div className="navbar">
        <div className="nav-inner">
          <Link to="/" className="brand" style={{ gap: 8 }}>
            <Logo withText />
          </Link>
          <div className="nav-actions">
            {role ? (
              <>
                <span className="muted small">Signed in as: {role}</span>
                <button className="btn btn-ghost small" onClick={handleLogout}>
                  Sign out
                </button>
              </>
            ) : (
              <>
                <Link className="link" to="/login">Login</Link>
                <Link className="link" to="/register">Register</Link>
              </>
            )}
          </div>
        </div>
      </div>

      {/* CONTENT */}
      <div className="container">
        {loading ? (
          <div className="card center" style={{ height: 120 }}>
            Loading session…
          </div>
        ) : bootError ? (
          <div className="card">
            <h2>We couldn’t validate your session</h2>
            <p className="muted small">{String(bootError)}</p>
          </div>
        ) : (
          <Routes>
            {/* LANDING PAGE */}
            <Route
              path="/"
              element={
                <div className="card hero" style={{ textAlign: "center", padding: "60px 30px" }}>
                  <div style={{ display: "flex", justifyContent: "center", marginBottom: 20 }}>
                    <Logo withText size={64} />
                  </div>

                  <h1 style={{ fontSize: 36, marginBottom: 12 }}>Fast. Secure. Global.</h1>
                  <p className="muted" style={{ fontSize: 16, marginBottom: 30 }}>
                    AtlasPay helps you send and receive international payments seamlessly,
                    with bank-level security and transparent verification.
                  </p>

                  <div style={{ display: "flex", justifyContent: "center", gap: 12 }}>
                    <Link to="/register" className="btn btn-primary">Create Account</Link>
                    <Link to="/login" className="btn btn-ghost">Sign In</Link>
                  </div>

                  <div style={{ marginTop: 40 }}>
                    <p className="small muted">Trusted by teams and individuals worldwide</p>
                    <div style={{ display: "flex", justifyContent: "center", gap: 16, marginTop: 10 }}>
                      <span className="badge">ISO-27001 Aligned</span>
                      <span className="badge">Bank-level Encryption</span>
                      <span className="badge">Real-time Status</span>
                    </div>
                  </div>
                </div>
              }
            />

            {/* AUTH + DASHBOARDS */}
            <Route path="/login" element={<Login onAuth={handleSetRole} />} />
            <Route path="/register" element={<Register onAuth={handleSetRole} />} />
            <Route
              path="/customer"
              element={
                <PrivateRoute allowed={["customer"]} role={role}>
                  <CustomerDashboard />
                </PrivateRoute>
              }
            />
            <Route
              path="/staff"
              element={
                <PrivateRoute allowed={["staff"]} role={role}>
                  <StaffDashboard />
                </PrivateRoute>
              }
            />
          </Routes>
        )}
      </div>
    </BrowserRouter>
  );
}
