using System;
using System.Windows.Forms;
using GymTrainerSystem.Data;
using GymTrainerSystem.Models;

namespace GymTrainerSystem.Forms
{
    public partial class BMICalculatorForm : Form
    {
        private Member member;

        public BMICalculatorForm(Member m)
        {
            member = m;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "BMI Calculator & Weight Update";
            this.Size = new System.Drawing.Size(420, 380);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblH = new Label { Text = "Height (cm):", Location = new System.Drawing.Point(50, 40), AutoSize = true };
            NumericUpDown numH = new NumericUpDown
            {
                Location = new System.Drawing.Point(180, 37),
                Width = 120,
                Minimum = 100,
                Maximum = 250,
                DecimalPlaces = 1,
                Value = member.HeightCm
            };

            Label lblW = new Label { Text = "Current Weight (kg):", Location = new System.Drawing.Point(50, 90), AutoSize = true };
            NumericUpDown numW = new NumericUpDown
            {
                Location = new System.Drawing.Point(180, 87),
                Width = 120,
                Minimum = 30,
                Maximum = 200,
                DecimalPlaces = 1,
                Value = member.CurrentWeightKg
            };

            Label lblResult = new Label
            {
                Name = "lblResult",
                Location = new System.Drawing.Point(50, 150),
                Size = new System.Drawing.Size(300, 80),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };

            Button btnCalc = new Button
            {
                Text = "Calculate BMI",
                Location = new System.Drawing.Point(50, 240),
                Width = 130,
                Height = 35
            };

            Button btnUpdate = new Button
            {
                Text = "Update & Log BMI",
                Location = new System.Drawing.Point(200, 240),
                Width = 140,
                Height = 35,
                BackColor = System.Drawing.Color.FromArgb(0, 150, 80),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnCalc.Click += (s, e) =>
            {
                decimal h = numH.Value / 100;
                decimal bmi = Math.Round(numW.Value / (h * h), 2);
                string cat = bmi < 18.5m ? "Underweight" :
                             bmi < 25m ? "Normal" :
                             bmi < 30m ? "Overweight" : "Obese";

                lblResult.Text = $"BMI = {bmi}\nCategory: {cat}\n\n" +
                                 (bmi < 18.5m ? "Suggestion: Focus on Weight Gain program." :
                                  bmi >= 25m ? "Suggestion: Focus on Weight Loss program." :
                                  "You are in healthy range. Maintain!");
            };

            btnUpdate.Click += (s, e) =>
            {
                if (DatabaseHelper.UpdateWeightAndLogBMI(member.MemberID, numW.Value, numH.Value, "Monthly update"))
                {
                    MessageBox.Show("Weight updated and BMI logged successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            this.Controls.AddRange(new Control[] { lblH, numH, lblW, numW, lblResult, btnCalc, btnUpdate });
        }
    }
}
