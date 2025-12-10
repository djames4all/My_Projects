// src/app.js
require("dotenv").config();
const express = require("express");
const helmet = require("helmet");
const cors = require("cors");
const cookieParser = require("cookie-parser");
const rateLimit = require("express-rate-limit");
const { ipKeyGenerator } = require("express-rate-limit");
const csurf = require("csurf");
const morgan = require("morgan");

const authRoutes = require("./routes/auth");
const paymentsRoutes = require("./routes/payments");
const beneficiariesRoutes = require("./routes/beneficiaries");

const app = express();

app.enable("trust proxy");

/* Security headers / CSP */
app.use(
  helmet({
    contentSecurityPolicy: {
      directives: {
        defaultSrc: ["'self'"],
        scriptSrc: ["'self'"],
        styleSrc: ["'self'"],
        frameAncestors: ["'none'"],
        baseUri: ["'self'"],
        fontSrc: ["'self'", "https:", "data:"],
        formAction: ["'self'"],
        imgSrc: ["'self'", "data:"],
        objectSrc: ["'none'"],
      },
    },
  })
);

/* CORS */
const isProd = process.env.NODE_ENV === "production";
const envOrigins = (process.env.CLIENT_ORIGINS || "")
  .split(",")
  .map((s) => s.trim())
  .filter(Boolean);

// In development, allow ANY https://localhost:<port> or http://localhost:<port>
function isDevLocalhost(origin = "") {
  try {
    const u = new URL(origin);
    return (
      u.hostname === "localhost" &&
      (u.protocol === "https:" || u.protocol === "http:")
    );
  } catch {
    return false;
  }
}

app.use(
  cors({
    origin: (origin, cb) => {
      if (!origin) return cb(null, true); // server-to-server/curl
      if (!isProd && isDevLocalhost(origin)) return cb(null, true);
      if (envOrigins.includes(origin)) return cb(null, true);
      return cb(new Error(`Not allowed by CORS: ${origin}`));
    },
    credentials: true,
    methods: ["GET", "POST", "PUT", "DELETE", "OPTIONS"],
    allowedHeaders: ["Content-Type", "Authorization", "X-CSRF-Token"],
  })
);

/* Parsers + Logging */
app.use(express.json({ limit: "10kb" }));
app.use(express.urlencoded({ extended: true, limit: "10kb" }));
app.use(cookieParser());
app.use(morgan(process.env.NODE_ENV === "production" ? "combined" : "dev"));

/* Dev Debug (Only non-production) */
if (!isProd) {
  app.use((req, res, next) => {
    console.log(
      `[DEBUG] ${new Date().toISOString()} ${req.method} ${
        req.originalUrl
      } origin:${req.get("origin") || "none"}`
    );
    console.log("[DEBUG] req.cookies:", req.cookies);
    console.log("[DEBUG] req.headers.cookie:", req.headers.cookie);
    next();
  });
}

/* Simple sanitizer */
function sanitizeObject(obj) {
  if (obj == null || typeof obj !== "object") return obj;
  if (Array.isArray(obj)) return obj.map(sanitizeObject);
  const out = {};
  for (const key of Object.keys(obj)) {
    const cleanKey = key.replace(/^\$+/, "_").replace(/\./g, "_");
    out[cleanKey] = sanitizeObject(obj[key]);
  }
  return out;
}
app.use((req, res, next) => {
  try {
    if (req.body && typeof req.body === "object")
      req.body = sanitizeObject(req.body);
    if (req.params && typeof req.params === "object")
      req.params = sanitizeObject(req.params);
    next();
  } catch (err) {
    next(err);
  }
});

/* Rate limiting (IPv6-safe key generator) */
const limiter = rateLimit({
  windowMs: 15 * 60 * 1000,
  max: 150,
  standardHeaders: true,
  legacyHeaders: false,
  keyGenerator: ipKeyGenerator,
});
app.use(limiter);

/* CSRF */
const isSecure =
  process.env.NODE_ENV === "production" || process.env.LOCAL_HTTPS === "true";
const sameSiteValue = isSecure ? "None" : "Lax";

app.use(
  csurf({
    cookie: {
      httpOnly: true,
      sameSite: sameSiteValue,
      secure: isSecure,
      maxAge: 2 * 60 * 60 * 1000,
      path: "/",
    },
    ignoreMethods: ["GET", "HEAD", "OPTIONS"],
  })
);

/* CSRF Token Endpoint */
app.get("/api/csrf-token", (req, res) => {
  try {
    const token = req.csrfToken();
    return res.json({ csrfToken: token });
  } catch (err) {
    const payload = { error: "CSRF token not available", detail: err?.message };
    if (!isProd) payload.stack = err?.stack;
    return res.status(500).json(payload);
  }
});

/* Routes */
app.use("/api/auth", authRoutes);
app.use("/api/payments", paymentsRoutes);
app.use("/api/beneficiaries", beneficiariesRoutes);

/* Error handler */
app.use((err, req, res, next) => {
  if (!isProd) console.error("ERROR:", err);
  if (err?.code === "EBADCSRFTOKEN") {
    return res.status(403).json({
      error: "Invalid CSRF token",
      csrf: { reason: err.message || "bad token" },
    });
  }
  const status = err?.status || 500;
  const message = err?.message || "Server error";
  const payload = { error: message };
  if (err?.errors && Array.isArray(err.errors)) payload.validation = err.errors;
  if (!isProd) payload.stack = err.stack;
  res.status(status).json(payload);
});

module.exports = app;
