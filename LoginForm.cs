using System;
using System.Windows.Forms;
using GymTrainerSystem.Data;
using GymTrainerSystem.Models;

namespace GymTrainerSystem.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Gym Trainer & Monitoring System - Login";
            this.Size = new System.Drawing.Size(420, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblTitle = new Label
            {
                Text = "GYM TRAINER & MONITOR",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(80, 25)
            };

            Label lblUser = new Label { Text = "Username:", Location = new System.Drawing.Point(60, 90), AutoSize = true };
            TextBox txtUser = new TextBox { Name = "txtUsername", Location = new System.Drawing.Point(150, 87), Width = 180 };

            Label lblPass = new Label { Text = "Password:", Location = new System.Drawing.Point(60, 130), AutoSize = true };
            TextBox txtPass = new TextBox { Name = "txtPassword", Location = new System.Drawing.Point(150, 127), Width = 180, PasswordChar = '*' };

            Button btnLogin = new Button
            {
                Text = "Login",
                Location = new System.Drawing.Point(150, 175),
                Width = 100,
                Height = 35,
                BackColor = System.Drawing.Color.FromArgb(0, 122, 204),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };

            Button btnRegister = new Button
            {
                Text = "New Admission",
                Location = new System.Drawing.Point(150, 220),
                Width = 100,
                Height = 30
            };

            Label lblInfo = new Label
            {
                Text = "Admin: admin / admin123\nUser : john / john123",
                Location = new System.Drawing.Point(60, 260),
                AutoSize = true,
                ForeColor = System.Drawing.Color.Gray,
                Font = new System.Drawing.Font("Segoe UI", 8)
            };

            btnLogin.Click += (s, e) =>
            {
                string username = txtUser.Text.Trim();
                string password = txtPass.Text;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter username and password.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                User user = DatabaseHelper.Login(username, password);
                if (user == null)
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.Hide();
                if (user.Role == "Admin")
                {
                    AdminDashboard admin = new AdminDashboard(user);
                    admin.FormClosed += (ss, ee) => this.Show();
                    admin.Show();
                }
                else
                {
                    UserDashboard dash = new UserDashboard(user);
                    dash.FormClosed += (ss, ee) => this.Show();
                    dash.Show();
                }
            };

            btnRegister.Click += (s, e) =>
            {
                RegistrationForm reg = new RegistrationForm();
                reg.ShowDialog();
            };

            this.Controls.AddRange(new Control[] { lblTitle, lblUser, txtUser, lblPass, txtPass, btnLogin, btnRegister, lblInfo });
            this.AcceptButton = btnLogin;
        }
    }
}
