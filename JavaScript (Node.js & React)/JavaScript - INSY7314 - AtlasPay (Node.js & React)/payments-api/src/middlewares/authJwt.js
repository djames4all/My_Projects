// src/middlewares/authJwt.js
const jwt = require("jsonwebtoken");
const User = require("../models/User");

const JWT_SECRET = process.env.JWT_SECRET || "dev_secret_change_me";

async function authJwt(req, res, next) {
  try {
    const token =
      req.cookies?.token || (req.header("Authorization") || "").split(" ")[1];

    if (!token) return res.status(401).json({ error: "No auth token" });

    const payload = jwt.verify(token, JWT_SECRET);

    // Accept either "id" or "sub" for compatibility
    const userId = payload.id || payload.sub;
    if (!userId)
      return res.status(401).json({ error: "Invalid token payload" });

    const user = await User.findById(userId).select(
      "-passwordHash -failedLoginCount -lockUntil"
    );
    if (!user) return res.status(401).json({ error: "Invalid token user" });

    req.user = user;
    next();
  } catch (err) {
    console.error("authJwt error:", err?.message || err);
    return res.status(401).json({ error: "Unauthorized" });
  }
}

function requireRole(role) {
  return (req, res, next) => {
    if (!req.user) return res.status(401).json({ error: "Unauthorized" });
    if (req.user.role !== role)
      return res.status(403).json({ error: "Forbidden" });
    next();
  };
}

module.exports = { authJwt, requireRole };
