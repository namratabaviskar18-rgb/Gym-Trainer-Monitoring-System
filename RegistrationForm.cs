using System;
using System.Windows.Forms;
using GymTrainerSystem.Data;
using GymTrainerSystem.Models;

namespace GymTrainerSystem.Forms
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Gym Admission / Registration";
            this.Size = new System.Drawing.Size(480, 520);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int y = 20;
            int labelX = 40;
            int inputX = 180;
            int width = 220;

            Label AddLabel(string text)
            {
                Label l = new Label { Text = text, Location = new System.Drawing.Point(labelX, y), AutoSize = true };
                this.Controls.Add(l);
                return l;
            }

            TextBox AddTextBox(string name)
            {
                TextBox t = new TextBox { Name = name, Location = new System.Drawing.Point(inputX, y - 3), Width = width };
                this.Controls.Add(t);
                y += 35;
                return t;
            }

            AddLabel("Full Name:");
            TextBox txtName = AddTextBox("txtName");

            AddLabel("Username:");
            TextBox txtUser = AddTextBox("txtUser");

            AddLabel("Password:");
            TextBox txtPass = AddTextBox("txtPass");
            txtPass.PasswordChar = '*';

            AddLabel("Email:");
            TextBox txtEmail = AddTextBox("txtEmail");

            AddLabel("Phone:");
            TextBox txtPhone = AddTextBox("txtPhone");

            AddLabel("Age:");
            NumericUpDown numAge = new NumericUpDown { Location = new System.Drawing.Point(inputX, y - 3), Width = 80, Minimum = 12, Maximum = 80, Value = 25 };
            this.Controls.Add(numAge);
            y += 35;

            AddLabel("Gender:");
            ComboBox cmbGender = new ComboBox { Location = new System.Drawing.Point(inputX, y - 3), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbGender.Items.AddRange(new object[] { "Male", "Female", "Other" });
            cmbGender.SelectedIndex = 0;
            this.Controls.Add(cmbGender);
            y += 35;

            AddLabel("Height (cm):");
            NumericUpDown numHeight = new NumericUpDown { Location = new System.Drawing.Point(inputX, y - 3), Width = 100, Minimum = 100, Maximum = 250, DecimalPlaces = 1, Value = 170 };
            this.Controls.Add(numHeight);
            y += 35;

            AddLabel("Current Weight (kg):");
            NumericUpDown numWeight = new NumericUpDown { Location = new System.Drawing.Point(inputX, y - 3), Width = 100, Minimum = 30, Maximum = 200, DecimalPlaces = 1, Value = 70 };
            this.Controls.Add(numWeight);
            y += 35;

            AddLabel("Target Weight (kg):");
            NumericUpDown numTarget = new NumericUpDown { Location = new System.Drawing.Point(inputX, y - 3), Width = 100, Minimum = 30, Maximum = 200, DecimalPlaces = 1, Value = 75 };
            this.Controls.Add(numTarget);
            y += 35;

            AddLabel("Goal:");
            ComboBox cmbGoal = new ComboBox { Location = new System.Drawing.Point(inputX, y - 3), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbGoal.Items.AddRange(new object[] { "WeightGain", "WeightLoss" });
            cmbGoal.SelectedIndex = 0;
            this.Controls.Add(cmbGoal);
            y += 45;

            Button btnSave = new Button
            {
                Text = "Register & Admit",
                Location = new System.Drawing.Point(inputX, y),
                Width = 140,
                Height = 35,
                BackColor = System.Drawing.Color.FromArgb(0, 150, 80),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            this.Controls.Add(btnSave);

            btnSave.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
                {
                    MessageBox.Show("Name, Username and Password are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (numTarget.Value == numWeight.Value)
                {
                    MessageBox.Show("Target weight should be different from current weight.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Auto correct goal type based on weights
                string goal = cmbGoal.SelectedItem.ToString();
                if (numTarget.Value > numWeight.Value && goal == "WeightLoss")
                    goal = "WeightGain";
                if (numTarget.Value < numWeight.Value && goal == "WeightGain")
                    goal = "WeightLoss";

                User user = new User
                {
                    Username = txtUser.Text.Trim(),
                    Password = txtPass.Text,
                    FullName = txtName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim()
                };

                Member member = new Member
                {
                    HeightCm = numHeight.Value,
                    CurrentWeightKg = numWeight.Value,
                    TargetWeightKg = numTarget.Value,
                    GoalType = goal,
                    Gender = cmbGender.SelectedItem.ToString(),
                    Age = (int)numAge.Value
                };

                if (DatabaseHelper.RegisterMember(user, member))
                {
                    MessageBox.Show("Registration successful!\nYou can now login with your username and password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Registration failed. Username may already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }
    }
}
