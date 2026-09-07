using System;
using System.Windows.Forms;
using GymTrainerSystem.Data;

namespace GymTrainerSystem.Forms
{
    public partial class WorkoutForm : Form
    {
        private string goalType;

        public WorkoutForm(string goal)
        {
            goalType = goal;
            InitializeComponent();
            LoadPlan();
        }

        private void InitializeComponent()
        {
            this.Text = $"Workout Plan - {goalType}";
            this.Size = new System.Drawing.Size(750, 480);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lbl = new Label
            {
                Text = $"Personalized Workout Schedule ({goalType})",
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 15),
                AutoSize = true
            };
            this.Controls.Add(lbl);

            DataGridView dgv = new DataGridView
            {
                Name = "dgvWorkout",
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(700, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.Color.White
            };
            this.Controls.Add(dgv);

            Button btnClose = new Button
            {
                Text = "Close",
                Location = new System.Drawing.Point(320, 415),
                Width = 100,
                Height = 30
            };
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private void LoadPlan()
        {
            DataGridView dgv = (DataGridView)this.Controls["dgvWorkout"];
            dgv.DataSource = DatabaseHelper.GetWorkoutPlan(goalType, "Beginner");
        }
    }
}
