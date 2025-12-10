// src/models/User.js
const mongoose = require("mongoose");
const userSchema = new mongoose.Schema(
  {
    fullName: { type: String, required: true },
    idNumber: { type: String, required: true },
    accountNumber: { type: String, required: true, index: true },
    email: { type: String, required: true, unique: true },
    passwordHash: { type: String, required: true },
    role: { type: String, enum: ["customer", "staff"], required: true },
  },
  { timestamps: true }
);
module.exports = mongoose.model("User", userSchema);
