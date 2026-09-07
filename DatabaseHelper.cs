using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using GymTrainerSystem.Models;

namespace GymTrainerSystem.Data
{
    public class DatabaseHelper
    {
        private static string connectionString =
            ConfigurationManager.ConnectionStrings["GymDB"]?.ConnectionString
            ?? "Data Source=.;Initial Catalog=GymTrainerDB;Integrated Security=True;TrustServerCertificate=True";

        // -------------------- LOGIN --------------------
        public static User Login(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Users WHERE Username=@u AND Password=@p";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return new User
                    {
                        UserID = Convert.ToInt32(dr["UserID"]),
                        Username = dr["Username"].ToString(),
                        FullName = dr["FullName"].ToString(),
                        Role = dr["Role"].ToString(),
                        Email = dr["Email"]?.ToString(),
                        Phone = dr["Phone"]?.ToString()
                    };
                }
            }
            return null;
        }

        // -------------------- REGISTER USER + MEMBER --------------------
        public static bool RegisterMember(User user, Member member)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    // Insert User
                    string userQuery = @"INSERT INTO Users (Username, Password, FullName, Role, Email, Phone)
                                         OUTPUT INSERTED.UserID
                                         VALUES (@u, @p, @n, 'User', @e, @ph)";
                    SqlCommand userCmd = new SqlCommand(userQuery, con, tran);
                    userCmd.Parameters.AddWithValue("@u", user.Username);
                    userCmd.Parameters.AddWithValue("@p", user.Password);
                    userCmd.Parameters.AddWithValue("@n", user.FullName);
                    userCmd.Parameters.AddWithValue("@e", (object)user.Email ?? DBNull.Value);
                    userCmd.Parameters.AddWithValue("@ph", (object)user.Phone ?? DBNull.Value);
                    int newUserId = (int)userCmd.ExecuteScalar();

                    // Insert Member
                    string memQuery = @"INSERT INTO Members (UserID, HeightCm, CurrentWeightKg, TargetWeightKg, GoalType, Gender, Age)
                                        VALUES (@uid, @h, @w, @t, @g, @gen, @age)";
                    SqlCommand memCmd = new SqlCommand(memQuery, con, tran);
                    memCmd.Parameters.AddWithValue("@uid", newUserId);
                    memCmd.Parameters.AddWithValue("@h", member.HeightCm);
                    memCmd.Parameters.AddWithValue("@w", member.CurrentWeightKg);
                    memCmd.Parameters.AddWithValue("@t", member.TargetWeightKg);
                    memCmd.Parameters.AddWithValue("@g", member.GoalType);
                    memCmd.Parameters.AddWithValue("@gen", (object)member.Gender ?? DBNull.Value);
                    memCmd.Parameters.AddWithValue("@age", member.Age);
                    memCmd.ExecuteNonQuery();

                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                }
            }
        }

        // -------------------- GET MEMBER BY USERID --------------------
        public static Member GetMemberByUserId(int userId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Members WHERE UserID=@id AND IsActive=1";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", userId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return new Member
                    {
                        MemberID = Convert.ToInt32(dr["MemberID"]),
                        UserID = Convert.ToInt32(dr["UserID"]),
                        HeightCm = Convert.ToDecimal(dr["HeightCm"]),
                        CurrentWeightKg = Convert.ToDecimal(dr["CurrentWeightKg"]),
                        TargetWeightKg = Convert.ToDecimal(dr["TargetWeightKg"]),
                        GoalType = dr["GoalType"].ToString(),
                        Gender = dr["Gender"]?.ToString(),
                        Age = Convert.ToInt32(dr["Age"]),
                        JoinDate = Convert.ToDateTime(dr["JoinDate"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"])
                    };
                }
            }
            return null;
        }

        // -------------------- UPDATE WEIGHT & LOG BMI --------------------
        public static bool UpdateWeightAndLogBMI(int memberId, decimal newWeight, decimal heightCm, string notes = null)
        {
            decimal heightM = heightCm / 100;
            decimal bmi = Math.Round(newWeight / (heightM * heightM), 2);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    // Update current weight
                    string upd = "UPDATE Members SET CurrentWeightKg=@w WHERE MemberID=@id";
                    SqlCommand cmd1 = new SqlCommand(upd, con, tran);
                    cmd1.Parameters.AddWithValue("@w", newWeight);
                    cmd1.Parameters.AddWithValue("@id", memberId);
                    cmd1.ExecuteNonQuery();

                    // Insert history
                    string ins = @"INSERT INTO BMIHistory (MemberID, WeightKg, BMI, Notes)
                                   VALUES (@mid, @w, @bmi, @n)";
                    SqlCommand cmd2 = new SqlCommand(ins, con, tran);
                    cmd2.Parameters.AddWithValue("@mid", memberId);
                    cmd2.Parameters.AddWithValue("@w", newWeight);
                    cmd2.Parameters.AddWithValue("@bmi", bmi);
                    cmd2.Parameters.AddWithValue("@n", (object)notes ?? DBNull.Value);
                    cmd2.ExecuteNonQuery();

                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                }
            }
        }

        // -------------------- GET BMI HISTORY --------------------
        public static List<BMIHistory> GetBMIHistory(int memberId)
        {
            List<BMIHistory> list = new List<BMIHistory>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM BMIHistory WHERE MemberID=@id ORDER BY RecordDate DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", memberId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new BMIHistory
                    {
                        HistoryID = Convert.ToInt32(dr["HistoryID"]),
                        MemberID = Convert.ToInt32(dr["MemberID"]),
                        WeightKg = Convert.ToDecimal(dr["WeightKg"]),
                        BMI = Convert.ToDecimal(dr["BMI"]),
                        RecordDate = Convert.ToDateTime(dr["RecordDate"]),
                        Notes = dr["Notes"]?.ToString()
                    });
                }
            }
            return list;
        }

        // -------------------- GET WORKOUT PLAN --------------------
        public static DataTable GetWorkoutPlan(string goalType, string level = "Beginner")
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT DayOfWeek, ExerciseName, Sets, Reps, DurationMin, Description
                                 FROM WorkoutPlans
                                 WHERE GoalType=@g AND Level=@l
                                 ORDER BY 
                                    CASE DayOfWeek
                                        WHEN 'Monday' THEN 1
                                        WHEN 'Tuesday' THEN 2
                                        WHEN 'Wednesday' THEN 3
                                        WHEN 'Thursday' THEN 4
                                        WHEN 'Friday' THEN 5
                                        WHEN 'Saturday' THEN 6
                                        WHEN 'Sunday' THEN 7
                                    END";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@g", goalType);
                cmd.Parameters.AddWithValue("@l", level);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // -------------------- GET DIET PLAN --------------------
        public static DataTable GetDietPlan(string goalType)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT MealType, FoodItem, Calories, Description
                                 FROM DietPlans WHERE GoalType=@g
                                 ORDER BY 
                                    CASE MealType
                                        WHEN 'Breakfast' THEN 1
                                        WHEN 'Lunch' THEN 2
                                        WHEN 'Snack' THEN 3
                                        WHEN 'Dinner' THEN 4
                                    END";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@g", goalType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // -------------------- ADMIN: GET ALL USERS --------------------
        public static DataTable GetAllUsers()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT u.UserID, u.Username, u.FullName, u.Role, u.Email, u.Phone,
                                        m.HeightCm, m.CurrentWeightKg, m.TargetWeightKg, m.GoalType, m.Age
                                 FROM Users u
                                 LEFT JOIN Members m ON u.UserID = m.UserID
                                 ORDER BY u.UserID";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // -------------------- ADMIN: DELETE USER --------------------
        public static bool DeleteUser(int userId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                // Cascade delete manually
                string q1 = "DELETE FROM BMIHistory WHERE MemberID IN (SELECT MemberID FROM Members WHERE UserID=@id)";
                string q2 = "DELETE FROM Members WHERE UserID=@id";
                string q3 = "DELETE FROM Users WHERE UserID=@id AND Role='User'"; // protect admin

                SqlCommand cmd1 = new SqlCommand(q1, con);
                cmd1.Parameters.AddWithValue("@id", userId);
                cmd1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand(q2, con);
                cmd2.Parameters.AddWithValue("@id", userId);
                cmd2.ExecuteNonQuery();

                SqlCommand cmd3 = new SqlCommand(q3, con);
                cmd3.Parameters.AddWithValue("@id", userId);
                return cmd3.ExecuteNonQuery() > 0;
            }
        }

        // -------------------- CHECK IF GOAL REACHED --------------------
        public static bool IsGoalReached(Member m)
        {
            if (m.GoalType == "WeightGain")
                return m.CurrentWeightKg >= m.TargetWeightKg;
            else
                return m.CurrentWeightKg <= m.TargetWeightKg;
        }
    }
}
