// src/models/Payment.js
const mongoose = require("mongoose");

const paymentSchema = new mongoose.Schema(
  {
    payerId: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User",
      required: true,
    },
    beneficiaryId: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "Beneficiary",
      required: true,
    },
    amount: { type: Number, required: true },
    currency: { type: String, required: true },
    provider: { type: String, default: "SWIFT" }, // hidden from customer
    status: {
      type: String,
      enum: ["pending", "verified", "declined"],
      default: "pending",
    },
    verifiedBy: { type: mongoose.Schema.Types.ObjectId, ref: "User" },
    verifiedAt: { type: Date },
  },
  { timestamps: true }
);

module.exports = mongoose.model("Payment", paymentSchema);
