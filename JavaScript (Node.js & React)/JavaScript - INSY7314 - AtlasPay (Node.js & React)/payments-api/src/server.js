// src/server.js
require("dotenv").config();
const https = require("https");
const fs = require("fs");
const path = require("path");
const mongoose = require("mongoose");
const app = require("./app");

const MONGO_URI = process.env.MONGO_URI;
const MONGO_DBNAME = process.env.MONGO_DBNAME;
const PORT = process.env.PORT || 4001; // <-- use 4001 if 4000 is busy
const USE_HTTPS = process.env.LOCAL_HTTPS === "true";

async function start() {
  try {
    if (!MONGO_URI) throw new Error("MONGO_URI missing in .env");
    await mongoose.connect(MONGO_URI, { dbName: MONGO_DBNAME });
    console.log("MongoDB connected");

    if (USE_HTTPS) {
      const key = fs.readFileSync(
        path.resolve(__dirname, "../certs/localhost.key")
      );
      const cert = fs.readFileSync(
        path.resolve(__dirname, "../certs/localhost.crt")
      );
      https.createServer({ key, cert }, app).listen(PORT, () => {
        console.log(`HTTPS server running on https://localhost:${PORT}`);
      });
    } else {
      app.listen(PORT, () => {
        console.log(`HTTP server running on http://localhost:${PORT}`);
      });
    }
  } catch (err) {
    console.error(err);
    process.exit(1);
  }
}

start();
