using System;
using System.Windows.Forms;
using GymTrainerSystem.Data;
using GymTrainerSystem.Models;

namespace GymTrainerSystem.Forms
{
    public partial class AdminDashboard : Form
    {
        private User admin;

        public AdminDashboard(User user)
        {
            admin = user;
            InitializeComponent();
            LoadUsers();
        }

        private void InitializeComponent()
        {
            this.Text = "Admin Dashboard - Manage Users";
            this.Size = new System.Drawing.Size(900, 520);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lbl = new Label
            {
                Text = $"Admin Panel - Welcome {admin.FullName}",
                Font = new System.Drawing.Font("Segoe UI", 13, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 15),
                AutoSize = true
            };
            this.Controls.Add(lbl);

            DataGridView dgv = new DataGridView
            {
                Name = "dgvUsers",
                Location = new System.Drawing.Point(20, 55),
                Size = new System.Drawing.Size(840, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.Color.White
            };
            this.Controls.Add(dgv);

            Button btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new System.Drawing.Point(20, 425),
                Width = 100,
                Height = 35
            };
            btnRefresh.Click += (s, e) => LoadUsers();
            this.Controls.Add(btnRefresh);

            Button btnDelete = new Button
            {
                Text = "Delete Selected User",
                Location = new System.Drawing.Point(140, 425),
                Width = 150,
                Height = 35,
                BackColor = System.Drawing.Color.FromArgb(180, 50, 50),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Select a user first.");
                    return;
                }
                int userId = Convert.ToInt32(dgv.SelectedRows[0].Cells["UserID"].Value);
                string role = dgv.SelectedRows[0].Cells["Role"].Value.ToString();
                if (role == "Admin")
                {
                    MessageBox.Show("Cannot delete Admin account.");
                    return;
                }
                if (MessageBox.Show("Are you sure you want to delete this user and all related data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (DatabaseHelper.DeleteUser(userId))
                    {
                        MessageBox.Show("User deleted.");
                        LoadUsers();
                    }
                    else
                        MessageBox.Show("Delete failed.");
                }
            };
            this.Controls.Add(btnDelete);

            Button btnLogout = new Button
            {
                Text = "Logout",
                Location = new System.Drawing.Point(760, 425),
                Width = 100,
                Height = 35
            };
            btnLogout.Click += (s, e) => this.Close();
            this.Controls.Add(btnLogout);
        }

        private void LoadUsers()
        {
            DataGridView dgv = (DataGridView)this.Controls["dgvUsers"];
            dgv.DataSource = DatabaseHelper.GetAllUsers();
        }
    }
}
