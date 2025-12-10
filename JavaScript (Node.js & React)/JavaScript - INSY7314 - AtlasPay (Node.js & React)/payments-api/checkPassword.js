// checkPassword.js
require("dotenv").config();
const mongoose = require("mongoose");
const User = require("./src/models/User");
const bcrypt = require("bcrypt");

async function run() {
  try {
    const uri = process.env.MONGO_URI;
    if (!uri) throw new Error("MONGO_URI missing in .env");
    await mongoose.connect(uri);
    const email = "cust1@example.com";
    const plain = "TestP@ss123!";
    const u = await User.findOne({ email }).lean();
    console.log("user found:", !!u);
    if (!u) {
      console.log("No user with email", email);
      process.exit(0);
    }
    const stored = u.passwordHash || u.password || "<no-password-field>";
    console.log("stored hash (first 40 chars):", String(stored).slice(0, 40));
    const match = await bcrypt.compare(plain, stored);
    console.log("password matches plain value:", match);
    process.exit(0);
  } catch (err) {
    console.error("Error:", err.message || err);
    process.exit(1);
  }
}

run();
