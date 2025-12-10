// src/controllers/authController.js
const bcrypt = require("bcryptjs");
const jwt = require("jsonwebtoken");
const User = require("../models/User");

// ===== Session policy =====
const ACCESS_TOKEN_TTL_MIN = Number(process.env.ACCESS_TTL_MIN || 20); // 20 minutes default
const JWT_SECRET = process.env.JWT_SECRET || "dev_secret_change_me";

function isSecureEnv() {
  return (
    process.env.NODE_ENV === "production" || process.env.LOCAL_HTTPS === "true"
  );
}
function cookieSameSite() {
  // Same-site SPA talking to same-origin API can use Lax.
  // If your API is on a different site/subdomain, set COOKIES_SAMESITE=None and ensure secure:true.
  return process.env.COOKIES_SAMESITE || "Lax";
}
function cookieOptions() {
  return {
    httpOnly: true,
    secure: isSecureEnv(),
    sameSite: cookieSameSite(),
    path: "/",
    maxAge: ACCESS_TOKEN_TTL_MIN * 60 * 1000, // ms
  };
}
function signAccessToken(user) {
  // IMPORTANT: use "id" (not "sub") to match authJwt middleware.
  const payload = { id: String(user._id), role: user.role, email: user.email };
  return jwt.sign(payload, JWT_SECRET, {
    expiresIn: `${ACCESS_TOKEN_TTL_MIN}m`,
  });
}

// ===== Controllers =====

// POST /api/auth/register   (customers only)
exports.register = async (req, res) => {
  try {
    const { fullName, email, password, accountNumber, idNumber } = req.body;
    if (!fullName || !email || !password || !accountNumber || !idNumber) {
      return res.status(400).json({ error: "Missing required fields" });
    }

    const existing = await User.findOne({
      email: (email || "").toLowerCase().trim(),
    });
    if (existing)
      return res.status(409).json({ error: "Email already in use" });

    const passwordHash = await bcrypt.hash(password, 12);
    const user = await User.create({
      fullName,
      email: (email || "").toLowerCase().trim(),
      passwordHash,
      accountNumber,
      idNumber,
      role: "customer",
    });

    // Rotate: set fresh short-lived cookie
    const token = signAccessToken(user);
    return res
      .cookie("token", token, cookieOptions())
      .status(201)
      .json({ user: { _id: user._id, email: user.email, role: user.role } });
  } catch (err) {
    console.error("Register error:", err);
    return res.status(500).json({ error: "Server error" });
  }
};

// POST /api/auth/login
exports.login = async (req, res) => {
  try {
    const { email, password } = req.body;
    const user = await User.findOne({
      email: (email || "").toLowerCase().trim(),
    });
    if (!user) return res.status(401).json({ error: "Invalid credentials" });

    const ok = await bcrypt.compare(password || "", user.passwordHash || "");
    if (!ok) return res.status(401).json({ error: "Invalid credentials" });

    // Rotate cookie on login
    const token = signAccessToken(user);
    return res
      .clearCookie("token", { path: "/" })
      .cookie("token", token, cookieOptions())
      .json({ user: { _id: user._id, email: user.email, role: user.role } });
  } catch (err) {
    console.error("Login error:", err);
    return res.status(500).json({ error: "Server error" });
  }
};

// GET /api/auth/me
exports.me = async (req, res) => {
  try {
    if (!req.user) return res.status(401).json({ error: "Not authenticated" });
    const u = req.user;
    return res.json({
      user: { _id: u._id, email: u.email, role: u.role, fullName: u.fullName },
    });
  } catch (err) {
    console.error("Me error:", err);
    return res.status(500).json({ error: "Server error" });
  }
};

// POST /api/auth/logout
exports.logout = async (_req, res) => {
  try {
    return res
      .clearCookie("token", { path: "/" })
      .status(200)
      .json({ ok: true });
  } catch (err) {
    console.error("Logout error:", err);
    return res.status(500).json({ error: "Server error" });
  }
};
