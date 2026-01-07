# Smart-Health-Insurance-Management-System

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


## Technology Stack

### Backend
- **ASP.NET Core Web API** – RESTful API development
- **C#** – Backend programming language
- **Entity Framework Core** – ORM for database access
- **LINQ** – Data querying and transformation
- **JWT (JSON Web Tokens)** – Authentication and role-based authorization

### Frontend
- **Angular** – Frontend framework
- **TypeScript** – Frontend programming language
- **Angular Material** – UI component library
- **HTML5 & SCSS** – UI structure and styling

### Database
- **SQL Server LocalDB** – Relational database

### Tools & Platforms
- **Visual Studio / Visual Studio Code** – Development IDEs
- **Node.js & npm** – Frontend dependency management
- **Swagger** – API documentation and testing
- **Git & GitHub** – Version control and source code management

----


## SetUp Instructions

### Project Setup Instructions
This project consists of two independent applications:
1. Backend: ASP.NET Core Web API
2. Frontend: Angular Web Application

### Backend Setup (ASP.NET Core Web API)

#### Step 1: Open Backend Solution
Navigate to the backend project folder
Open the .sln file using Visual Studio or open the folder in VS Code. 

#### Step 2: Configure Database Connection
Open the appsettings.json file in the ASP.NET Core Web API project and verify the following
configurations. 

```

"JwtSettings": { "Issuer": "HealthInsuranceApi", "Audience": "HealthInsuranceApiUsers", 
"SecretKey": "THIS_IS_A_VERY_SECURE_SECRET_KEY_12345", 
"TokenExpiryMinutes": 60
}"
ConnectionStrings": { "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=HealthInsuranceDB10;Trusted_Connection=True;"
}

"Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning"
}
},
AllowedHosts": "*"
```


#### Step 3: Apply Database Migrations
Open Package Manager Console or terminal and run:
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```
This will create the database and required tables. 

### NOTE: After migrating the database kindly remove the foreign key FK_CustomerProfiles_AgentProfiles_AgentProfileId from CustomerProfiles Table in the database before execution of the application.

#### Step 4: Run Backend API
Press F5 or run:
```
dotnet run
```

#### Step 5: Verify API
Open Swagger UI in the browser

### Frontend Setup (Angular)

## Step 1: Open Frontend Project
Navigate to the Angular project directory.
Open in VS Code. 

## Step 2: Install Dependencies
Run the following command:
```
npm install
```

## Step 3: Run Angular Application
Start the development server:
```
ng serve
```

### DEFAULT ADMIN CREDENTIALS
- Username: admin
- Password: Admin@123

(Pre-seeded during database initialization)



