# Agri-Energy Connect – Functional Prototype Guide

Agri-Energy Connect is a web-based prototype application for managing farmers and their agricultural products through a role-based access system. The system offers different functionalities to two user roles: **Farmers** and **Employees**. This README walks through how the system works, what each role can do, and how the main features are used in the application.

---

## 🧑‍💻 How the App Works

### 👤 User Roles & Login

Upon navigating to the site, users are presented with a landing page (`/`) featuring login/register options.

#### ➤ Authentication
- The app uses **ASP.NET Identity** for secure login.
- Users must log in to access any core functionality.
- Based on the user role (Farmer or Employee), the system displays specific views and hides restricted features.

#### ➤ Roles
- **Farmer**
  - Can add and view **their own** products.
- **Employee**
  - Can add **new farmer profiles**.
  - Can view and **filter all products** submitted by all farmers.

---

## 🧭 User Journey

### 🔵 1. Home Page (`/`)
- Visitors see a welcome message, banner images, and role-based login/register buttons.
- If already logged in:
  - Farmers are redirected to their **Product Dashboard**.
  - Employees are redirected to the **Farmer Management Panel**.

---

### 🟢 2. Farmer Features

#### ➤ Dashboard (`/Products/Index`)
- Displays a list of products **added by the logged-in farmer**.
- Uses a custom-styled table with product details: Name, Category, and Production Date.
- Actions:
  - **Edit** – Update an existing product.
  - **Details** – View full product info.
  - **Delete** – Remove product.

#### ➤ Add Product (`/Products/Create`)
- Form to add a new product.
- Fields: Product Name, Category (dropdown), Production Date.
- Validation:
  - All fields required.
  - Date must be valid and not in the future.

---

### 🟡 3. Employee Features

#### ➤ Farmer List (`/Farmers/Index`)
- Displays all registered farmers in the system.
- Actions:
  - **Create Farmer** – Opens a form to add a new farmer (name, contact, etc.).
  - **View Products** – Takes the employee to the filtered product list for that farmer.

#### ➤ Add Farmer (`/Farmers/Create`)
- Adds a new farmer profile to the database.
- Fields: Full Name, Region, Contact Info.
- Validation included to prevent duplicates or blank entries.

#### ➤ View Products by Farmer (`/Products/ByFarmer/{farmerId}`)
- Employee can see **only the products** tied to the selected farmer.
- A filter bar lets employees:
  - Filter by **date range** (start to end).
  - Filter by **product type** (category dropdown).

---

## 📂 Frontend Behavior

- Site uses **Razor Views** with server-side rendering.
- Conditional logic in `_Layout.cshtml` checks `User.Identity.IsAuthenticated` and user roles to:
  - Show/hide navigation menu links.
  - Redirect to appropriate views post-login.
  
### 📱 Responsiveness
- Layout is mobile-friendly with clean, modern CSS styles.
- The home page (`Index.cshtml`) includes:
  - Banner carousel
  - Welcome section
  - Feature highlights
  - "Go to Dashboard" button (authenticated users only)

---

## 🔐 Access Control

Controllers enforce access using `[Authorize]` and custom logic:
| `ProductsController` | Farmers (their data only) & Employees (all data) |
| `FarmersController`  | Employees only |

- Role checks are done using:
  ```csharp
  if (User.IsInRole("Farmer")) { ... }
  if (User.IsInRole("Employee")) { ... }
