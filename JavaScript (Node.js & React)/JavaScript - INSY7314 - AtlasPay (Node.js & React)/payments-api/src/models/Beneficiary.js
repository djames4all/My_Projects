// src/models/Beneficiary.js
const mongoose = require("mongoose");

const beneficiarySchema = new mongoose.Schema(
  {
    name: { type: String, required: true },
    bankName: { type: String, required: true },
    beneficiaryAccountNumber: { type: String, required: true },

    // The customer who owns/uses this beneficiary
    ownerId: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User",
      index: true,
      required: true,
    },

    // (Optional) Which staff user created this beneficiary
    createdBy: { type: mongoose.Schema.Types.ObjectId, ref: "User" },

    // Optional SWIFT information
    swiftCode: { type: String },
  },
  { timestamps: true }
);

module.exports = mongoose.model("Beneficiary", beneficiarySchema);
