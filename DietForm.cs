using System;
using System.Windows.Forms;
using GymTrainerSystem.Data;

namespace GymTrainerSystem.Forms
{
    public partial class DietForm : Form
    {
        private string goalType;

        public DietForm(string goal)
        {
            goalType = goal;
            InitializeComponent();
            LoadPlan();
        }

        private void InitializeComponent()
        {
            this.Text = $"Diet Consultancy - {goalType}";
            this.Size = new System.Drawing.Size(700, 420);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lbl = new Label
            {
                Text = $"Recommended Diet Plan ({goalType})",
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 15),
                AutoSize = true
            };
            this.Controls.Add(lbl);

            DataGridView dgv = new DataGridView
            {
                Name = "dgvDiet",
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(640, 290),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.Color.White
            };
            this.Controls.Add(dgv);

            Label note = new Label
            {
                Text = "Note: Consult a certified nutritionist for personalized adjustments. Drink 3-4 litres of water daily.",
                Location = new System.Drawing.Point(20, 350),
                Size = new System.Drawing.Size(640, 30),
                ForeColor = System.Drawing.Color.DarkSlateGray
            };
            this.Controls.Add(note);
        }

        private void LoadPlan()
        {
            DataGridView dgv = (DataGridView)this.Controls["dgvDiet"];
            dgv.DataSource = DatabaseHelper.GetDietPlan(goalType);
        }
    }
}
