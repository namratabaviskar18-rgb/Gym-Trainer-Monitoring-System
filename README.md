# Gym Trainer & Monitoring System

Complete C# Windows Forms + SQL Server project based on the given presentation.

## Features Implemented

### User Module
- Login
- Gym Admission / Registration
- BMI Calculation (with category)
- Update weight & automatic monthly BMI logging
- Workout Suggestion (Weight Gain / Weight Loss schedules)
- Diet Consultancy
- Goal tracking (system congratulates when target weight is reached)

### Admin Module
- Login
- View all members with details
- Delete users

## Technology Stack
- **Language**: C# (.NET Framework 4.7.2 or later)
- **UI**: Windows Forms
- **Database**: Microsoft SQL Server
- **IDE**: Visual Studio 2019 / 2022

## How to Run

### 1. Create the Database
1. Open **SQL Server Management Studio** (SSMS)
2. Connect to your SQL Server instance
3. Open and execute the script:  
   `Database/GymDB.sql`
4. This creates `GymTrainerDB` with tables + sample data

### 2. Open Project in Visual Studio
1. Create a new **Windows Forms App (.NET Framework)** project named `GymTrainerSystem`
2. Delete the default Form1
3. Add all the folders and `.cs` files from this repository into the project
4. Add reference: **System.Configuration**
5. Make sure `App.config` is included and set as content
6. Change the connection string in `App.config` if your SQL Server instance name is different

### 3. Default Login Credentials

| Role  | Username | Password  |
|-------|----------|-----------|
| Admin | admin    | admin123  |
| User  | john     | john123   |

### 4. Build & Run
Press **F5** in Visual Studio.

## Project Structure
```
GymTrainerSystem/
├── Database/
│   └── GymDB.sql          ← Run this first
├── Models/
│   ├── User.cs
│   ├── Member.cs
│   └── BMIHistory.cs
├── Data/
│   └── DatabaseHelper.cs  ← All database operations
├── Forms/
│   ├── LoginForm.cs
│   ├── RegistrationForm.cs
│   ├── UserDashboard.cs
│   ├── BMICalculatorForm.cs
│   ├── WorkoutForm.cs
│   ├── DietForm.cs
│   └── AdminDashboard.cs
├── Program.cs
├── App.config
└── README.md
```

## Notes
- Passwords are stored in plain text for simplicity (student project). In real applications use hashing (BCrypt / ASP.NET Identity).
- Workout and Diet plans are seeded for "Beginner" level. You can easily extend levels in the database.
- The system automatically detects goal completion and shows a congratulatory message.
