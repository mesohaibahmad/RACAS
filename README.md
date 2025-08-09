# RACAS – Reporting and Analysis of Costs for Assembly and Support

## 📌 Overview
**RACAS** is a **Payment Request Management System** designed for the multinational company. It automates the submission, review, and approval of payment requests between **External Assembly Partners (EAPs)** and internal staff.

The system replaces the existing Excel-based workflow with a **web-based, secure, role-driven, and branch-aware platform**. It streamlines financial request processing, enforces approval workflows, maintains complete audit trails, and provides reporting with **machine learning-based trend predictions** for better financial planning.

---

## 🎯 Objectives
- Automate **payment request workflows** to reduce manual errors.
- Provide **role-based and branch-specific access control**.
- Maintain **logs and audit history** for transparency.
- Deliver **real-time dashboards and reports**.
- Integrate **ML models** for financial forecasting.

---

## 👥 User Roles & Permissions
| Role        | Key Responsibilities |
|-------------|----------------------|
| **SCMA** (Service Center Employee) | Submit and manage payment requests. |
| **SCL, AL, HL/VKL** | Perform control checks on requests. |
| **VTL, LL** | Approve requests for payment. |
| **Admin** | Manage users, branches, dropdowns, and system settings. |

---

## 🏗️ System Architecture
**Frontend:** Vue.js integrated with ASP.NET Core MVC Razor views, Bootstrap, Chart.js  
**Backend:** ASP.NET Core MVC, Entity Framework Core, MySQL  
**Deployment:** IIS / Cloud Hosting (Azure, AWS, or equivalent)  
**ML Integration:** .Net ML for predictive analytics  

---

## ⚙️ Features  

### **Core Functionalities**
- **User Authentication & Role Management** (MS Identity/JWT-based).
- **Payment Request CRUD** (Create, Read, Update, Delete).
- **Approval Workflow** – Status flow: `NEW` → `PENDING CONTROL CHECK` → `CONTROL CHECK OK` → `APPROVED FOR PAYMENT`.
- **Branch-Specific Permissions** – Users can only access requests for their branch.
- **EAP-Branch Relationship Management** – Track requests linked to specific assembly partners.
- **Comments & Attachments** – For request clarification and document uploads.
- **Audit Logs** – Complete history of all changes.

### **Reporting & Dashboard**
- Income vs. Expense graphs.
- Machine learning predictions for future trends.
- Export reports in PDF/Excel formats.

---

## 📊 Workflow Diagram
**High-Level Payment Request Lifecycle:**
1. User logs in.
2. Creates payment request with attachments.
3. Request undergoes control check.
4. If approved, moves to payment-ready status.
5. System logs each action and updates dashboard metrics.

---

## 🛠️ Installation & Setup

### **1. Prerequisites**
- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Git

### **2. Clone the Repository**
```bash
git clone https://github.com/mesohaibahmad/RACAS.git
cd RACAS
```

### **3. Setup**
```bash
cd RACAS
dotnet restore
dotnet ef database update
dotnet run
```
#### **Login Credentials:** 
username: admin
password: 123

---

## 📈 Machine Learning Integration
- Historical financial data is exported from SQL.
- .Net ML models (ARIMA / Linear Regression) generate predictions.
- Predictions are stored in the database and displayed in the dashboard.

---

## ✅ Testing
- **Unit Tests:** xUnit for backend services.
- **Integration Tests:** Postman collections with staging DB.
- **Manual UI Testing:** Cross-browser and responsive checks.
- **User Acceptance Testing:** Simulated real-world scenarios with sample data.

---

## 📜 License
This project is for academic purposes only and is not licensed for commercial use without permission.

---

