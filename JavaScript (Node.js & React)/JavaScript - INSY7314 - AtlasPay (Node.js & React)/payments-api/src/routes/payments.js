// src/routes/payments.js
const express = require("express");
const { body, param, validationResult } = require("express-validator");
const Payment = require("../models/Payment");
const Beneficiary = require("../models/Beneficiary");
const { authJwt, requireRole } = require("../middlewares/authJwt");

const router = express.Router();

// ===== Currency allow-list (expand here when needed) =====
const CURRENCIES = [
  "USD",
  "EUR",
  "GBP",
  "ZAR",
  "AUD",
  "CAD",
  "NZD",
  "JPY",
  "CNY",
  "INR",
  "CHF",
  "SEK",
  "NOK",
  "DKK",
  "AED",
  "SAR",
  "HKD",
  "SGD",
];

function handleValidationErrors(req, res, next) {
  const errors = validationResult(req);
  if (errors.isEmpty()) return next();
  return res.status(400).json({
    error: "Validation error",
    details: errors.array().map((e) => ({ field: e.path, message: e.msg })),
  });
}

/**
 * Create payment (Customer)
 * - Customer can pay ANY beneficiary.
 * - If no beneficiaryId and none exist, create default.
 */
router.post(
  "/create",
  authJwt,
  requireRole("customer"),
  [
    body("amount")
      .isFloat({ gt: 0 })
      .withMessage("amount must be a number greater than 0"),
    body("currency")
      .isIn(CURRENCIES)
      .withMessage(`currency must be one of: ${CURRENCIES.join(", ")}`),
    body("beneficiaryId")
      .optional()
      .isMongoId()
      .withMessage("beneficiaryId must be a valid MongoId"),
  ],
  handleValidationErrors,
  async (req, res) => {
    try {
      const { amount, currency, beneficiaryId } = req.body;

      let beneficiary;
      if (beneficiaryId) {
        beneficiary = await Beneficiary.findById(beneficiaryId);
        if (!beneficiary)
          return res.status(404).json({ error: "Beneficiary not found" });
      } else {
        beneficiary = await Beneficiary.findOne().sort({ createdAt: -1 });
        if (!beneficiary) {
          beneficiary = await Beneficiary.create({
            ownerId: req.user._id,
            name: "Default Beneficiary",
            bankName: "Default Bank",
            beneficiaryAccountNumber: "00000000",
          });
        }
      }

      const payment = await Payment.create({
        payerId: req.user._id,
        beneficiaryId: beneficiary._id,
        amount: Number(amount),
        currency,
        provider: "SWIFT",
        payeeAccount: beneficiary.beneficiaryAccountNumber || "00000000",
        payeeSwift: beneficiary.swiftCode || undefined,
        status: "pending",
      });

      res.status(201).json(payment);
    } catch (err) {
      console.error("Create payment error:", err);
      res.status(500).json({ error: "Server error" });
    }
  }
);

// Customer: my payments
router.get(
  "/my-payments",
  authJwt,
  requireRole("customer"),
  async (req, res) => {
    try {
      const payments = await Payment.find({ payerId: req.user._id })
        .populate("beneficiaryId", "name bankName beneficiaryAccountNumber")
        .sort({ createdAt: -1 });

      const safe = payments.map((p) => ({
        _id: p._id,
        payerId: p.payerId,
        beneficiaryId: p.beneficiaryId,
        amount: p.amount,
        currency: p.currency,
        provider: p.provider,
        payeeAccount: p.payeeAccount,
        status: p.status,
        createdAt: p.createdAt,
        updatedAt: p.updatedAt,
        verifiedAt: p.verifiedAt,
        verifiedBy: p.verifiedBy,
      }));

      res.json(safe);
    } catch (err) {
      console.error("Get my payments error:", err);
      res.status(500).json({ error: "Server error" });
    }
  }
);

// Staff: all payments
router.get("/all", authJwt, requireRole("staff"), async (_req, res) => {
  try {
    const payments = await Payment.find()
      .populate("beneficiaryId payerId", "name email")
      .sort({ createdAt: -1 });
    res.json(payments);
  } catch (err) {
    console.error("Get all payments error:", err);
    res.status(500).json({ error: "Server error" });
  }
});

// Staff: verify/decline payment
router.put(
  "/verify/:id",
  authJwt,
  requireRole("staff"),
  [
    param("id").isMongoId().withMessage("id must be a valid MongoId"),
    body("status")
      .isIn(["verified", "declined"])
      .withMessage("status must be verified or declined"),
  ],
  handleValidationErrors,
  async (req, res) => {
    try {
      const payment = await Payment.findById(req.params.id);
      if (!payment) return res.status(404).json({ error: "Payment not found" });

      const { status } = req.body;
      payment.status = status;
      if (status === "verified") {
        payment.verifiedAt = new Date();
        payment.verifiedBy = req.user._id;
      } else {
        payment.verifiedAt = undefined;
        payment.verifiedBy = undefined;
      }
      await payment.save();

      res.json(payment);
    } catch (err) {
      console.error("Verify payment error:", err);
      res.status(500).json({ error: "Server error" });
    }
  }
);

module.exports = router;
