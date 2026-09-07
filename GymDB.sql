-- =============================================
-- Gym Trainer & Monitoring System - Database
-- SQL Server Script
-- =============================================

CREATE DATABASE GymTrainerDB;
GO

USE GymTrainerDB;
GO

-- Users table (Admin + Members)
CREATE TABLE Users (
    UserID          INT IDENTITY(1,1) PRIMARY KEY,
    Username        NVARCHAR(50)  NOT NULL UNIQUE,
    Password        NVARCHAR(100) NOT NULL,          -- In production use hashing
    FullName        NVARCHAR(100) NOT NULL,
    Role            NVARCHAR(20)  NOT NULL CHECK (Role IN ('Admin','User')),
    Email           NVARCHAR(100),
    Phone           NVARCHAR(20),
    CreatedAt       DATETIME DEFAULT GETDATE()
);

-- Members profile (linked to Users)
CREATE TABLE Members (
    MemberID        INT IDENTITY(1,1) PRIMARY KEY,
    UserID          INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    HeightCm        DECIMAL(5,2) NOT NULL,           -- in cm
    CurrentWeightKg DECIMAL(5,2) NOT NULL,           -- in kg
    TargetWeightKg  DECIMAL(5,2) NOT NULL,
    GoalType        NVARCHAR(20) NOT NULL CHECK (GoalType IN ('WeightGain','WeightLoss')),
    Gender          NVARCHAR(10),
    Age             INT,
    JoinDate        DATETIME DEFAULT GETDATE(),
    IsActive        BIT DEFAULT 1
);

-- Monthly BMI tracking
CREATE TABLE BMIHistory (
    HistoryID       INT IDENTITY(1,1) PRIMARY KEY,
    MemberID        INT NOT NULL FOREIGN KEY REFERENCES Members(MemberID),
    WeightKg        DECIMAL(5,2) NOT NULL,
    BMI             DECIMAL(5,2) NOT NULL,
    RecordDate      DATETIME DEFAULT GETDATE(),
    Notes           NVARCHAR(200)
);

-- Workout Plans
CREATE TABLE WorkoutPlans (
    PlanID          INT IDENTITY(1,1) PRIMARY KEY,
    GoalType        NVARCHAR(20) NOT NULL,           -- WeightGain / WeightLoss
    Level           NVARCHAR(20) NOT NULL,           -- Beginner / Intermediate / Advanced
    DayOfWeek       NVARCHAR(15) NOT NULL,
    ExerciseName    NVARCHAR(100) NOT NULL,
    Sets            INT,
    Reps            NVARCHAR(20),
    DurationMin     INT,
    Description     NVARCHAR(300)
);

-- Diet Plans
CREATE TABLE DietPlans (
    DietID          INT IDENTITY(1,1) PRIMARY KEY,
    GoalType        NVARCHAR(20) NOT NULL,
    MealType        NVARCHAR(30) NOT NULL,           -- Breakfast / Lunch / Dinner / Snack
    FoodItem        NVARCHAR(150) NOT NULL,
    Calories        INT,
    Description     NVARCHAR(300)
);

-- =============================================
-- Seed Data
-- =============================================

-- Default Admin
INSERT INTO Users (Username, Password, FullName, Role, Email)
VALUES ('admin', 'admin123', 'System Administrator', 'Admin', 'admin@gym.com');

-- Sample User
INSERT INTO Users (Username, Password, FullName, Role, Email, Phone)
VALUES ('john', 'john123', 'John Doe', 'User', 'john@email.com', '9876543210');

INSERT INTO Members (UserID, HeightCm, CurrentWeightKg, TargetWeightKg, GoalType, Gender, Age)
VALUES (2, 175.00, 65.00, 75.00, 'WeightGain', 'Male', 25);

-- Sample Workout Plans - Weight Gain
INSERT INTO WorkoutPlans (GoalType, Level, DayOfWeek, ExerciseName, Sets, Reps, DurationMin, Description) VALUES
('WeightGain','Beginner','Monday','Bench Press',4,'8-10',45,'Focus on progressive overload'),
('WeightGain','Beginner','Monday','Incline Dumbbell Press',3,'10-12',NULL,NULL),
('WeightGain','Beginner','Monday','Tricep Pushdown',3,'12-15',NULL,NULL),
('WeightGain','Beginner','Tuesday','Squats',4,'8-10',50,'Keep back straight'),
('WeightGain','Beginner','Tuesday','Leg Press',3,'10-12',NULL,NULL),
('WeightGain','Beginner','Tuesday','Calf Raises',4,'15-20',NULL,NULL),
('WeightGain','Beginner','Wednesday','Pull-ups / Lat Pulldown',4,'8-10',45,NULL),
('WeightGain','Beginner','Wednesday','Barbell Rows',3,'10-12',NULL,NULL),
('WeightGain','Beginner','Wednesday','Bicep Curls',3,'12-15',NULL,NULL),
('WeightGain','Beginner','Thursday','Rest / Light Cardio',NULL,NULL,30,'Active recovery'),
('WeightGain','Beginner','Friday','Overhead Press',4,'8-10',45,NULL),
('WeightGain','Beginner','Friday','Lateral Raises',3,'12-15',NULL,NULL),
('WeightGain','Beginner','Saturday','Deadlift',3,'6-8',50,'Proper form is critical'),
('WeightGain','Beginner','Sunday','Rest',NULL,NULL,NULL,'Full recovery');

-- Sample Workout Plans - Weight Loss
INSERT INTO WorkoutPlans (GoalType, Level, DayOfWeek, ExerciseName, Sets, Reps, DurationMin, Description) VALUES
('WeightLoss','Beginner','Monday','Jumping Jacks',NULL,NULL,10,'Warm-up'),
('WeightLoss','Beginner','Monday','Burpees',3,'12-15',NULL,NULL),
('WeightLoss','Beginner','Monday','Mountain Climbers',3,'20',NULL,NULL),
('WeightLoss','Beginner','Monday','Plank',3,'30-45 sec',NULL,NULL),
('WeightLoss','Beginner','Tuesday','Running / Cycling',NULL,NULL,30,'Moderate intensity'),
('WeightLoss','Beginner','Wednesday','Bodyweight Squats',4,'15-20',40,NULL),
('WeightLoss','Beginner','Wednesday','Lunges',3,'12 each leg',NULL,NULL),
('WeightLoss','Beginner','Wednesday','Push-ups',3,'10-15',NULL,NULL),
('WeightLoss','Beginner','Thursday','HIIT Circuit',NULL,NULL,25,'30s work / 30s rest'),
('WeightLoss','Beginner','Friday','Swimming / Brisk Walk',NULL,NULL,40,NULL),
('WeightLoss','Beginner','Saturday','Full Body Circuit',NULL,NULL,35,NULL),
('WeightLoss','Beginner','Sunday','Rest / Yoga',NULL,NULL,20,'Stretching & recovery');

-- Sample Diet Plans - Weight Gain
INSERT INTO DietPlans (GoalType, MealType, FoodItem, Calories, Description) VALUES
('WeightGain','Breakfast','Oats + Banana + Peanut Butter + Eggs',650,'High calorie start'),
('WeightGain','Lunch','Chicken Breast + Brown Rice + Vegetables',700,NULL),
('WeightGain','Snack','Protein Shake + Handful of Nuts',400,NULL),
('WeightGain','Dinner','Salmon / Paneer + Sweet Potato + Salad',650,NULL),
('WeightGain','Snack','Greek Yogurt + Honey + Berries',300,NULL);

-- Sample Diet Plans - Weight Loss
INSERT INTO DietPlans (GoalType, MealType, FoodItem, Calories, Description) VALUES
('WeightLoss','Breakfast','Egg Whites + Spinach + Whole Wheat Toast',350,'High protein low calorie'),
('WeightLoss','Lunch','Grilled Chicken Salad + Quinoa',450,NULL),
('WeightLoss','Snack','Apple + Handful of Almonds',200,NULL),
('WeightLoss','Dinner','Fish / Tofu + Steamed Vegetables',400,NULL),
('WeightLoss','Snack','Green Tea + Cucumber',50,NULL);

PRINT 'Database GymTrainerDB created successfully with seed data.';
GO
