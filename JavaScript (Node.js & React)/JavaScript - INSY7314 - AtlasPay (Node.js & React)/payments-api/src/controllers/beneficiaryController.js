// src/controllers/beneficiaryController.js
const Beneficiary = require("../models/Beneficiary");

exports.create = async (req, res) => {
  try {
    const { name, beneficiaryAccountNumber, bankName, swiftCode } = req.body;
    if (!name || !beneficiaryAccountNumber)
      return res.status(400).json({ error: "Missing fields" });

    const b = await Beneficiary.create({
      customerId: req.user._id,
      name,
      beneficiaryAccountNumber,
      bankName,
      swiftCode,
    });
    res.status(201).json(b);
  } catch (err) {
    console.error("beneficiaries.create", err);
    res.status(500).json({ error: "Server error" });
  }
};

exports.list = async (req, res) => {
  try {
    const filter = {};
    if (req.user.role === "customer") filter.customerId = req.user._id;
    const items = await Beneficiary.find(filter)
      .sort({ createdAt: -1 })
      .limit(500);
    res.json(items);
  } catch (err) {
    console.error("beneficiaries.list", err);
    res.status(500).json({ error: "Server error" });
  }
};
