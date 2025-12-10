// src/routes/auth.js
const express = require("express");
const { body } = require("express-validator");
const router = express.Router();
const authController = require("../controllers/authController");
const { authJwt } = require("../middlewares/authJwt");

// Register (force server-side role)
router.post(
  "/register",
  [
    body("fullName").isLength({ min: 3 }).trim().escape(),
    body("email").isEmail().normalizeEmail(),
    body("password").isStrongPassword({
      minLength: 12,
      minLowercase: 1,
      minUppercase: 1,
      minNumbers: 1,
      minSymbols: 1,
    }),
    body("accountNumber").matches(/^\d{8,18}$/),
    body("idNumber").matches(/^\d{13}$/),
  ],
  (req, res, next) => {
    req.body.role = "customer";
    return authController.register(req, res, next);
  }
);

// Login
router.post(
  "/login",
  [
    body("email").isEmail().normalizeEmail(),
    body("password").isLength({ min: 6 }),
  ],
  authController.login
);

// Logout
router.post("/logout", authController.logout);

// Me (validate cookie)
router.get("/me", authJwt, authController.me);

module.exports = router;
