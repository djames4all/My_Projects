// src/seed.js
require("dotenv").config();
const mongoose = require("mongoose");
const bcrypt = require("bcryptjs");

const User = require("./models/User");
const Beneficiary = require("./models/Beneficiary");
const Payment = require("./models/Payment");

const uri =
  process.env.MONGO_URI || "mongodb://127.0.0.1:27017/international-paymentsDB";

(async () => {
  try {
    await mongoose.connect(uri);
    console.log("✅ Connected to MongoDB, seeding demo data...");

    // --- Create staff ---
    const adminEmail = "admin@example.com";
    const staff = await User.findOneAndUpdate(
      { email: adminEmail },
      {
        fullName: "Seed Admin",
        idNumber: "0000000000000",
        accountNumber: "0000000000",
        email: adminEmail,
        passwordHash: await bcrypt.hash("Admin@123", 12),
        role: "staff",
      },
      { upsert: true, new: true }
    );

    // --- Create customer ---
    const custEmail = "cust@example.com";
    const customer = await User.findOneAndUpdate(
      { email: custEmail },
      {
        fullName: "Seed Customer",
        idNumber: "8001010000000",
        accountNumber: "10000001",
        email: custEmail,
        passwordHash: await bcrypt.hash("Customer@123", 12),
        role: "customer",
      },
      { upsert: true, new: true }
    );

    // --- Create beneficiary ---
    const beneficiary = await Beneficiary.findOneAndUpdate(
      { name: "ACME Ltd" },
      {
        name: "ACME Ltd",
        bankName: "Local Bank",
        beneficiaryAccountNumber: "12345678",
        swiftCode: "BANKZAJJXXX",
        ownerId: customer._id,
      },
      { upsert: true, new: true }
    );

    // --- Create sample payment ---
    await Payment.findOneAndUpdate(
      { payerId: customer._id, amount: 250.5 },
      {
        payerId: customer._id,
        beneficiaryId: beneficiary._id,
        amount: 250.5,
        currency: "USD",
        provider: "SWIFT",
        payeeAccount: beneficiary.beneficiaryAccountNumber,
        payeeSwift: beneficiary.swiftCode,
        status: "pending",
      },
      { upsert: true, new: true }
    );

    console.log("🎉 Seeding complete!");
  } catch (err) {
    console.error("❌ Seeding error:", err);
    process.exit(1);
  } finally {
    await mongoose.disconnect();
    console.log("🔌 Disconnected from MongoDB.");
  }
})();
