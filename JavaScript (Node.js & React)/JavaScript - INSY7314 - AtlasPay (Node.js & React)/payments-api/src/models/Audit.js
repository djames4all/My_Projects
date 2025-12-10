// src/models/Audit.js
const mongoose = require("mongoose");
const s = new mongoose.Schema(
  {
    userId: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User",
      required: true,
    },
    action: { type: String, required: true },
    targetType: String,
    targetId: mongoose.Schema.Types.ObjectId,
    ip: String,
    userAgent: String,
    meta: Object,
  },
  { timestamps: true }
);
module.exports = mongoose.model("Audit", s);
