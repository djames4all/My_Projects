// src/routes/beneficiaries.js
const express = require("express");
const { body, validationResult } = require("express-validator");
const Beneficiary = require("../models/Beneficiary");
const { authJwt } = require("../middlewares/authJwt");

const router = express.Router();

// helper: send validation errors consistently
function handleValidationErrors(req, res, next) {
  const errors = validationResult(req);
  if (errors.isEmpty()) return next();
  return res.status(400).json({
    error: "Validation error",
    details: errors.array().map((e) => ({
      field: e.path,
      message: e.msg,
    })),
  });
}

/**
 * Create a beneficiary
 * - Staff may set ownerId explicitly (required).
 * - Customers have ownerId forced to themselves.
 * Input whitelisting:
 *   name: 2-80 chars
 *   bankName: 2-80 chars
 *   beneficiaryAccountNumber: digits only, length 6-30
 *   swiftCode (optional): BIC format 8 or 11 alnum uppercase
 *   ownerId (if provided): must be a MongoId
 */
router.post(
  "/",
  authJwt,
  [
    body("name")
      .isString()
      .trim()
      .isLength({ min: 2, max: 80 })
      .withMessage("name must be 2-80 characters"),
    body("bankName")
      .isString()
      .trim()
      .isLength({ min: 2, max: 80 })
      .withMessage("bankName must be 2-80 characters"),
    body("beneficiaryAccountNumber")
      .matches(/^[0-9]{6,30}$/)
      .withMessage("beneficiaryAccountNumber must be 6-30 digits"),
    body("swiftCode")
      .optional({ nullable: true })
      .matches(/^[A-Z0-9]{8}([A-Z0-9]{3})?$/)
      .withMessage(
        "swiftCode must be a valid BIC (8 or 11 uppercase letters/numbers)"
      ),
    body("ownerId")
      .optional()
      .isMongoId()
      .withMessage("ownerId must be a valid MongoId"),
  ],
  handleValidationErrors,
  async (req, res) => {
    try {
      const { name, beneficiaryAccountNumber, bankName, swiftCode } = req.body;

      const resolvedOwnerId =
        req.user.role === "staff" ? req.body.ownerId || null : req.user._id;

      if (!resolvedOwnerId) {
        return res
          .status(400)
          .json({
            error: "ownerId is required for staff-created beneficiaries",
          });
      }

      const beneficiary = await Beneficiary.create({
        name,
        beneficiaryAccountNumber,
        bankName,
        swiftCode,
        ownerId: resolvedOwnerId,
        createdBy: req.user._id,
      });

      res.status(201).json(beneficiary);
    } catch (err) {
      console.error("Create beneficiary error:", err);
      res.status(500).json({ error: "Server error" });
    }
  }
);

/**
 * List ALL beneficiaries
 * (Requirement: allow customers to select any beneficiary)
 */
router.get("/", authJwt, async (_req, res) => {
  try {
    const all = await Beneficiary.find().sort({ createdAt: -1 });
    res.json(all);
  } catch (err) {
    console.error("List beneficiaries error:", err);
    res.status(500).json({ error: "Server error" });
  }
});

module.exports = router;
