import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import fs from "fs";
import path from "path";

export default defineConfig({
  plugins: [react()],
  server: {
    https: {
      key: fs.readFileSync(
        path.resolve(__dirname, "../payments-api/certs/localhost.key")
      ),
      cert: fs.readFileSync(
        path.resolve(__dirname, "../payments-api/certs/localhost.crt")
      ),
    },
    host: "localhost",
    port: 5174, // ← you were running on 5174; keep it consistent
    proxy: {
      // Proxy API calls to Express (which should also be running with HTTPS)
      "/api": {
        target: "https://localhost:4000",
        changeOrigin: true,
        secure: false, // allow self-signed cert in dev
      },
    },
  },
});
