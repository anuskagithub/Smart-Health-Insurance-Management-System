## Smart-Health-Insurance-Management-System

This is end-to-end process flow of the Smart Health Insurance Management System. It explains how different users interact with the system and how data flows from policy creation to claim settlement. The system is designed as a full-stack web application using ASP.NET Core Web API (backend) and Angular (frontend), with secure rolebased access.

## User Roles

The Health Insurance Management System supports the following user roles, each with specific responsibilities and access permissions.

### 1. Admin
- Manages system users and roles
- Approves and activates registered users
- Creates and manages insurance plans
- Assigns Claims Officers and Hospital Providers
- Monitors overall system activity and reports

---

### 2. Customer
- Registers and logs into the system
- Views available insurance plans
- Purchases and manages policies
- Submits insurance claims
- Tracks claim status and policy details

---

### 3. Claims Officer
- Views assigned insurance claims
- Reviews claim details and supporting information
- Approves or rejects claims based on policy rules
- Updates claim status and remarks
- Views claim analytics and reports

---

### 4. Hospital Provider
- Views claims associated with the hospital
- Submits treatment history for claims
- Updates medical and treatment details
- Supports claim verification process

----


### SETUP INSTRUCTIONS


### BACKEND SETUP (ASP.NET Core API)
### Step 1: Navigate to backend project directory
cd backend

### Step 2: Open appsettings.json and verify configuration
 (No command required – manual verification)
 Ensure the following values are present:
 ```
 JwtSettings:
   Issuer: HealthInsuranceApi
   Audience: HealthInsuranceApiUsers
   SecretKey: THIS_IS_A_VERY_SECURE_SECRET_KEY_12345
    TokenExpiryMinutes: 60

 ConnectionStrings:
   DefaultConnection:
   Server=(localdb)\MSSQLLocalDB;
   Database=HealthInsuranceDB10;
   Trusted_Connection=True;
```

### Step 3: Apply Entity Framework migrations
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### NOTE:
 Before running the application, remove the foreign key
 FK_CustomerProfiles_AgentProfiles_AgentProfileId
 from the CustomerProfiles table in the database.

### Step 4: Run the backend API
dotnet run

 Backend will start on a local HTTPS port.
 Verify using Swagger:
 https://localhost:<port>/swagger


 -------------------------------
# FRONTEND SETUP (ANGULAR)
# -------------------------------

# Step 5: Navigate to frontend project directory
cd ../frontend

# Step 6: Install project dependencies
npm install

# Step 7: Run Angular development server
ng serve

# Frontend will be available at:
# http://localhost:4200


# -------------------------------
# DEFAULT ADMIN CREDENTIALS
# -------------------------------
# Username: admin
# Password: Admin@123
# (Pre-seeded during database initialization)



