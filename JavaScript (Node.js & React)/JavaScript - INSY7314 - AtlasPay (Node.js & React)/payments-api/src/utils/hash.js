// src/utils/hash.js
const bcrypt = require("bcrypt");
const SALT_ROUNDS = parseInt(process.env.SALT_ROUNDS || "12", 10);
async function hashPassword(plain) {
  return bcrypt.hash(plain, SALT_ROUNDS);
}
async function comparePassword(plain, hash) {
  return bcrypt.compare(plain, hash);
}
module.exports = { hashPassword, comparePassword };
