using System;
using System.Windows.Forms;
using GymTrainerSystem.Data;
using GymTrainerSystem.Models;

namespace GymTrainerSystem.Forms
{
    public partial class UserDashboard : Form
    {
        private User currentUser;
        private Member currentMember;

        public UserDashboard(User user)
        {
            currentUser = user;
            currentMember = DatabaseHelper.GetMemberByUserId(user.UserID);
            InitializeComponent();
            LoadProfile();
        }

        private void InitializeComponent()
        {
            this.Text = "User Dashboard - Gym Trainer System";
            this.Size = new System.Drawing.Size(700, 480);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Header
            Label lblWelcome = new Label
            {
                Name = "lblWelcome",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblWelcome);

            // Profile Panel
            GroupBox grpProfile = new GroupBox
            {
                Text = "My Profile & BMI",
                Location = new System.Drawing.Point(30, 60),
                Size = new System.Drawing.Size(300, 280)
            };
            this.Controls.Add(grpProfile);

            Label lblInfo = new Label
            {
                Name = "lblInfo",
                Location = new System.Drawing.Point(15, 30),
                Size = new System.Drawing.Size(270, 230),
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            grpProfile.Controls.Add(lblInfo);

            // Buttons
            int btnX = 370;
            int btnY = 80;
            int btnW = 250;
            int btnH = 40;
            int gap = 50;

            Button btnBMI = CreateButton("BMI Calculator & Update Weight", btnX, btnY, btnW, btnH);
            btnBMI.Click += (s, e) =>
            {
                BMICalculatorForm f = new BMICalculatorForm(currentMember);
                f.FormClosed += (ss, ee) => { RefreshMember(); LoadProfile(); };
                f.ShowDialog();
            };
            this.Controls.Add(btnBMI);
            btnY += gap;

            Button btnWorkout = CreateButton("Workout Suggestion", btnX, btnY, btnW, btnH);
            btnWorkout.Click += (s, e) =>
            {
                if (currentMember == null) return;
                WorkoutForm f = new WorkoutForm(currentMember.GoalType);
                f.ShowDialog();
            };
            this.Controls.Add(btnWorkout);
            btnY += gap;

            Button btnDiet = CreateButton("Diet Consultancy", btnX, btnY, btnW, btnH);
            btnDiet.Click += (s, e) =>
            {
                if (currentMember == null) return;
                DietForm f = new DietForm(currentMember.GoalType);
                f.ShowDialog();
            };
            this.Controls.Add(btnDiet);
            btnY += gap;

            Button btnHistory = CreateButton("View BMI History (Monthly Track)", btnX, btnY, btnW, btnH);
            btnHistory.Click += (s, e) =>
            {
                if (currentMember == null) return;
                var history = DatabaseHelper.GetBMIHistory(currentMember.MemberID);
                if (history.Count == 0)
                {
                    MessageBox.Show("No BMI records yet. Update your weight first.", "Info");
                    return;
                }
                string msg = "BMI Tracking History:\n\n";
                foreach (var h in history)
                {
                    msg += $"{h.RecordDate:dd-MMM-yyyy}  |  Weight: {h.WeightKg} kg  |  BMI: {h.BMI}\n";
                }
                MessageBox.Show(msg, "Monthly BMI Track");
            };
            this.Controls.Add(btnHistory);
            btnY += gap;

            Button btnLogout = CreateButton("Logout", btnX, btnY, btnW, btnH);
            btnLogout.BackColor = System.Drawing.Color.FromArgb(180, 50, 50);
            btnLogout.Click += (s, e) => this.Close();
            this.Controls.Add(btnLogout);

            // Goal status label
            Label lblGoal = new Label
            {
                Name = "lblGoalStatus",
                Location = new System.Drawing.Point(30, 360),
                Size = new System.Drawing.Size(620, 50),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            this.Controls.Add(lblGoal);
        }

        private Button CreateButton(string text, int x, int y, int w, int h)
        {
            return new Button
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(w, h),
                BackColor = System.Drawing.Color.FromArgb(0, 122, 204),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9)
            };
        }

        private void RefreshMember()
        {
            currentMember = DatabaseHelper.GetMemberByUserId(currentUser.UserID);
        }

        private void LoadProfile()
        {
            Label lblWelcome = (Label)this.Controls["lblWelcome"];
            Label lblInfo = (Label)((GroupBox)this.Controls[1]).Controls["lblInfo"];
            Label lblGoal = (Label)this.Controls["lblGoalStatus"];

            lblWelcome.Text = $"Welcome, {currentUser.FullName}!";

            if (currentMember == null)
            {
                lblInfo.Text = "No member profile found.";
                return;
            }

            lblInfo.Text =
                $"Name      : {currentUser.FullName}\n" +
                $"Age       : {currentMember.Age}\n" +
                $"Gender    : {currentMember.Gender}\n" +
                $"Height    : {currentMember.HeightCm} cm\n" +
                $"Weight    : {currentMember.CurrentWeightKg} kg\n" +
                $"Target    : {currentMember.TargetWeightKg} kg\n" +
                $"Goal      : {currentMember.GoalType}\n" +
                $"BMI       : {currentMember.BMI} ({currentMember.BMICategory})\n" +
                $"To Change : {currentMember.WeightToChange} kg";

            if (DatabaseHelper.IsGoalReached(currentMember))
            {
                lblGoal.Text = "🎉 CONGRATULATIONS! You have reached your target weight. Cycle completed!";
                lblGoal.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                string action = currentMember.GoalType == "WeightGain" ? "gain" : "lose";
                lblGoal.Text = $"Keep going! You still need to {action} {currentMember.WeightToChange} kg to reach your goal.";
                lblGoal.ForeColor = System.Drawing.Color.DarkOrange;
            }
        }
    }
}
