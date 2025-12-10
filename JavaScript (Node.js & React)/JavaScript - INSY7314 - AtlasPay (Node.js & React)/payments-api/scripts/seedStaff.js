// payments-api/scripts/seedStaff.js
require("dotenv").config();
const mongoose = require("mongoose");
const User = require("../src/models/User");
const Beneficiary = require("../src/models/Beneficiary");
const Payment = require("../src/models/Payment");
const Audit = require("../src/models/Audit");
const { hashPassword } = require("../src/utils/hash");

async function ensureUser(email, fullName, pwdPlain, role = "customer") {
  let u = await User.findOne({ email });
  if (!u) {
    u = await User.create({
      fullName,
      email,
      passwordHash: await hashPassword(pwdPlain),
      role,
      accountNumber: role === "staff" ? "0000000000" : "10000001",
      idNumber: role === "staff" ? "0000000000000" : "8001010000000",
    });
    console.log(`Created ${role}: ${email}`);
  } else {
    console.log(`${role} exists: ${email}`);
  }
  return u;
}

async function run() {
  try {
    if (!process.env.MONGO_URI) throw new Error("MONGO_URI missing in .env");
    await mongoose.connect(process.env.MONGO_URI, {
      dbName: process.env.MONGO_DBNAME,
    });
    console.log("Mongo connected");

    const staff = await ensureUser(
      "admin@example.com",
      "Seed Admin",
      "StaffP@ssw0rd!",
      "staff"
    );

    const cust = await ensureUser(
      "cust@example.com",
      "Seed Customer",
      "CustP@ssw0rd!",
      "customer"
    );

    // Ensure a beneficiary exists for the seeded customer
    let b = await Beneficiary.findOne({ beneficiaryAccountNumber: "12345678" });
    if (!b) {
      b = await Beneficiary.create({
        ownerId: cust._id, // matches Beneficiary schema
        name: "ACME Ltd",
        beneficiaryAccountNumber: "12345678",
        bankName: "Local Bank",
        swiftCode: "LOCALBIC1",
      });
      console.log("Created beneficiary: 12345678");
    } else {
      console.log("Beneficiary exists: 12345678");
    }

    // Ensure a payment exists and satisfies required schema fields (payerId etc.)
    let p = await Payment.findOne({ beneficiaryId: b._id, payerId: cust._id });
    if (!p) {
      p = await Payment.create({
        payerId: cust._id, // required by your Payment schema
        customerId: cust._id, // keep for backward compatibility if used elsewhere
        beneficiaryId: b._id,
        amount: 250.5,
        currency: "USD",
        provider: "SWIFT",
        payeeAccount: b.beneficiaryAccountNumber,
        payeeSwift: b.swiftCode,
        status: "pending",
      });
      console.log("Created payment:", p._id.toString());

      await Audit.create({
        userId: cust._id,
        action: "create_payment",
        targetType: "Payment",
        targetId: p._id,
        ip: "127.0.0.1",
        userAgent: "seed-script",
        meta: { amount: p.amount, currency: p.currency },
      });
      console.log("Audit for create_payment added");
    } else {
      console.log("Payment exists:", p._id.toString());
    }

    console.log("Seeding complete");
    process.exit(0);
  } catch (err) {
    console.error("Seed error", err);
    process.exit(1);
  }
}

run();
