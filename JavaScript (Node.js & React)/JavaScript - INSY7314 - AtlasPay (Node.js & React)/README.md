# AtlasPay (INSY7314)

Secure, full-stack demo for international payments with **Customer** and **Staff** portals.  
Tech: **React + Vite** · **Node/Express** · **MongoDB**
---

## Youtube Links
- Part 2 Youtube video link: https://youtu.be/E2Oq3FDgPeQ
- Part 3 Youtube video link: https://youtu.be/pypowvlmCeM

---
## Student numbers
- Ayush Harduth – ST10365068
- Dashayin Naicker – ST10378422
- Gia Bagwandeen – ST10365002
- Daniel James – ST10393280
---

## SonarQube and CircleCi
- The application passed the tests and here is proof of that
- Showing CircleCi
<img width="691" height="379" alt="image" src="https://github.com/user-attachments/assets/3c780115-d0bc-4072-b287-16bf22676eb1" />

- Showing SonarQube
<img width="698" height="383" alt="image" src="https://github.com/user-attachments/assets/4ad40043-8eea-4f38-90f1-e7dbcf1cf96a" />



---

## ✨ Features
- Customer register/login; create payments to **any** beneficiary
- Staff create beneficiaries; **verify/decline** payments
- Clean, responsive dark UI with helpful validation

---

## 🗂️ Project Structure
- **Client:** `payments-client/` (Vite, React Router, Axios)
- **API:** `payments-api/` (Express, Mongoose)
- **DB:** MongoDB (Users, Beneficiaries, Payments)

Main flows:
1) Customer → register/login → create payment → view history  
2) Staff → login → create beneficiaries → verify/decline payments

---

## ⚙️ Quick Start
1. **API**
   - Open terminal → `payments-api/`
   - Install deps → `npm i`
   - Create `.env` with:  
     `MONGO_URI`, `JWT_SECRET`, `ACCESS_TTL_MIN=20`, `LOCAL_HTTPS=true`, `COOKIES_SAMESITE=Lax`, `PORT=4000`
   - Run → `npm run dev`
2. **Client**
   - New terminal → `payments-client/`
   - Install deps → `npm i`
   - Run → `npm run dev`
3. Open the HTTPS localhost URL printed by Vite.

> **Prod tip:** put the API behind an HTTPS proxy (Nginx/Caddy/Traefik) and enable HSTS/CSP.

---

## 🔐 Attacks Prevented (brief)
- **CSRF** → Token flow (`X-CSRF-Token`)  
  _Client:_ `payments-client/src/services/api.js` · _Server:_ `payments-api/src/app.js`
- **Injection / malformed input** → Server-side validation & allow-lists  
  _Payments:_ `payments-api/src/routes/payments.js` · _Beneficiaries:_ `payments-api/src/routes/beneficiaries.js`
- **Session hijacking/fixation** → Short-lived **HttpOnly** cookie, **Secure/SameSite** flags, **rotation** on login/logout  
  _Auth:_ `payments-api/src/controllers/authController.js` · _JWT verify/RBAC:_ `payments-api/src/middlewares/authJwt.js`
- **Unauthorized access** → Role-based access (`requireRole('customer'|'staff')`) + guarded routes  
  _Server:_ `payments-api/src/middlewares/authJwt.js` · _Client:_ `payments-client/src/components/PrivateRoute.jsx`
- **DoS/abuse (baseline)** → Global rate limiting  
  _Server:_ `payments-api/src/app.js`
- **Clickjacking/XSS/CSP/HSTS (prod optional)** → Helmet security headers  
  _Server:_ `payments-api/src/app.js`

---

## 👤 Demo Accounts
- **Staff:** `admin@example.com` (password in seed/DB)
- **Customers:** sample users in MongoDB (e.g., `cust@example.com`)

---

## ✅ Quick Test (1 minute)
- Submit invalid payment (e.g., text amount) → expect **400** with clear message  
- Check cookie flags in DevTools → **HttpOnly**, **SameSite**, short **Expires** (~20m)  
- Remove `X-CSRF-Token` on a POST → request rejected  
- As customer, hit `/payments/all` → **403** (RBAC)

---

## 📄 License
Educational project for INSY7314 POE.
