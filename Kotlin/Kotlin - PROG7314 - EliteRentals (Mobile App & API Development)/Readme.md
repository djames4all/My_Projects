# 🏢 Elite Rentals – Mobile App

Elite Rentals is a fully featured **property management Android application** built using **Kotlin**. It enables tenants, property managers, caretakers, and administrators to interact through a centralized system.

The app provides tools for managing rentals, maintenance requests, user accounts, proof-of-payment submissions, and more — all streamlined into role-based dashboards.

---

**Group Name:** Fantastic 4  

**Group Members:**
- Dashayin Naicker: ST10378422  
- Gia Bagwandeen: ST10365002  
- Ayush Harduth: ST103650068  
- Daniel James: ST10393280  

---

## 🔗 Project Links
- 🎥 **Video showing functionality:** https://drive.google.com/file/d/1DJzEwYP1nzaCEsMqrxXNUfKZ56Cawqp5/view?usp=sharing
- ⚙️ **REST API (Swagger):** [https://eliterentalsapi-czckh7fadmgbgtgf.southafricanorth-01.azurewebsites.net/swagger/index.html](https://eliterentalsapi-czckh7fadmgbgtgf.southafricanorth-01.azurewebsites.net/swagger/index.html)  
- GitHub Repo: https://github.com/VCSTDN2024/prog7314-poe-EliteRentals.git
---

## 📌 1. Project Overview

Elite Rentals digitizes and modernizes rental operations by consolidating workflows into a secure mobile app.  
The system supports **multiple user types**, each with unique permissions and interfaces:

- **Tenants** – payments & maintenance  
- **Property Managers** – oversight & approvals  
- **Caretakers** – assigned work orders  
- **Administrators** – full platform control  

---

## ⭐ 2. Key Features (Fully Explained)

### 🔐 Authentication & Secure Access
- **JWT-Based Login**  
- **Role-Based Routing**  
- **User Account Management (CRUD + approvals)**  

### 🧑‍💼 Tenant Features
- Dashboard with payments & lease info  
- Submit maintenance requests with images  
- Upload proof of payment  

### 🏢 Property Manager & Admin Features
- Property management tools  
- Tenant approval workflows  
- Enable/disable accounts  
- Export PDF reports  

### 🧰 Caretaker Features
- View assigned tasks  
- Update task progress  

### 📶 Offline Support
- Queued offline actions  
- Local caching for faster loading  

---

## 🧱 3. Architecture & Design Considerations

### 🧩 MVVM Architecture
- **Model** – data + repository logic  
- **View** – Activity/XML  
- **ViewModel** – business logic  

### 📚 Repository Pattern
Manages API (Retrofit) and local database (Room).

### 🌐 Retrofit + Coroutines
Clean and modern async API handling.

### 🎨 Material Design UI
- Cards, buttons, shadows, themes  

### 📡 Offline-First Design
Ensures usability even with unstable networks.

---
## 🚀 4. Building & Running the App

### Requirements
- Android Studio
- Kotlin
- Gradle

### Steps
- git clone https://github.com/VCSTDN2024/prog7314-poe-EliteRentals.git
- Open in Android Studio
- Run

### Gradle Commands
- ./gradlew assembleDebug
- ./gradlew installDebug


## 🔐 5. Security & Data Handling

- Encrypted token storage  
- HTTPS enforced  
- Role-based access control  
- API response validation  

---

## 🤖 6. GitHub & GitHub Actions (CI/CD)

### Why GitHub?
Central version control, collaboration, code reviews.

### Why GitHub Actions?
Automates:
- Builds  
- Tests  
- Lint checks  

### CI Workflow
1. Code pushed  
2. Environment setup  
3. Build APK  
4. Run tests  
5. Lint check  
6. PR marked pass/fail  

---

## 📘 7. Development Best Practices

- Follow MVVM  
- Avoid logic in Activities  
- Use descriptive commit messages  
- Don’t store secrets in GitHub  
- Consistent Material Design usage  

---

## 🛠 8. Troubleshooting

### ❗ Login 401  
- Expired token  
- Wrong credentials  
- Backend offline  

### ❗ CI Build Failure  
- Wrong JDK/Gradle versions  

### ❗ API Call Failures  
- Incorrect base URL  
- Missing Internet permission  
