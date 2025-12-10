// src/controllers/paymentController.js
const Payment = require("../models/Payment");
const Beneficiary = require("../models/Beneficiary");

// Create a new payment
exports.createPayment = async (req, res) => {
  try {
    const { beneficiaryId, amount, currency } = req.body;
    const payerId = req.user._id; // Assuming you have auth middleware that sets req.user

    // Check if beneficiary exists
    const beneficiary = await Beneficiary.findById(beneficiaryId);
    if (!beneficiary) {
      return res.status(404).json({ message: "Beneficiary not found" });
    }

    // Create payment (status pending by default)
    const payment = new Payment({
      payerId,
      beneficiaryId,
      amount,
      currency,
      status: "pending",
    });

    await payment.save();

    res.status(201).json({ message: "Payment created successfully", payment });
  } catch (err) {
    console.error(err);
    res.status(500).json({ message: "Server error" });
  }
};

// Get all payments for logged-in customer
exports.getMyPayments = async (req, res) => {
  try {
    const payerId = req.user._id;

    const payments = await Payment.find({ payerId })
      .populate("beneficiaryId", "name beneficiaryAccountNumber bankName") // populate basic info only
      .sort({ createdAt: -1 });

    // Map to hide SWIFT and flatten beneficiary info
    const response = payments.map((tx) => ({
      _id: tx._id,
      beneficiaryId: tx.beneficiaryId._id,
      beneficiaryName: tx.beneficiaryId.name,
      beneficiaryAccountNumber: tx.beneficiaryId.beneficiaryAccountNumber,
      bankName: tx.beneficiaryId.bankName,
      amount: tx.amount,
      currency: tx.currency,
      status: tx.status,
      createdAt: tx.createdAt,
      updatedAt: tx.updatedAt,
    }));

    res.json(response);
  } catch (err) {
    console.error(err);
    res.status(500).json({ message: "Server error" });
  }
};
