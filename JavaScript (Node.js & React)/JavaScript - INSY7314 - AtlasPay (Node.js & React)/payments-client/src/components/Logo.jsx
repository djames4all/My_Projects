// src/components/Logo.jsx
import React from "react";

export default function Logo({ withText = false, size = 28 }) {
  const s = Number(size);
  return (
    <div style={{ display: "inline-flex", alignItems: "center", gap: 10 }}>
      <svg
        width={s}
        height={s}
        viewBox="0 0 32 32"
        xmlns="http://www.w3.org/2000/svg"
        aria-label="AtlasPay"
        style={{ borderRadius: 8, display: "block" }}
      >
        <defs>
          <linearGradient id="ap-g" x1="0" y1="0" x2="1" y2="1">
            <stop offset="0%" stopColor="#6ea8fe" />
            <stop offset="100%" stopColor="#22d3ee" />
          </linearGradient>
          <radialGradient id="ap-glow" cx="50%" cy="35%" r="70%">
            <stop offset="0%" stopColor="rgba(255,255,255,0.55)" />
            <stop offset="100%" stopColor="rgba(255,255,255,0)" />
          </radialGradient>
        </defs>

        {/* base rounded square */}
        <rect x="1.5" y="1.5" width="29" height="29" rx="8" fill="url(#ap-g)" />
        {/* inner glow */}
        <circle cx="12" cy="10" r="9" fill="url(#ap-glow)" />
        {/* subtle cut and depth */}
        <path d="M4 22 C12 18, 20 28, 28 18 L28 30 L4 30 Z" fill="rgba(0,0,0,0.18)" />
        {/* A-shaped mark */}
        <path
          d="M10.5 17.5l4.2-8.8a1.1 1.1 0 012 0l4.2 8.8a1.1 1.1 0 01-1 1.6h-8.5a1.1 1.1 0 01-1-1.6z"
          fill="rgba(8,16,28,.55)"
        />
        <circle cx="16" cy="16" r="2.4" fill="#0b1118" opacity="0.85" />
      </svg>

      {withText && (
        <span
          style={{
            fontWeight: 800,
            letterSpacing: ".2px",
            fontSize: 18,
            color: "#e6edf3",
          }}
        >
          AtlasPay
        </span>
      )}
    </div>
  );
}
