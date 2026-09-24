using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmLogin : Form
    {
        private Label lblUsername, lblPassword, lblStatus, lblTitle, lblIcon;
        private TextBox txtUsername, txtPassword;
        private Button btnLogin, btnChangePassword;
        private Panel loginPanel;

        public frmLogin() => InitializeComponent();

        private void InitializeComponent()
        {
            loginPanel = new Panel
            {
                Size = new Size(420, 520),
                Location = new Point((Screen.PrimaryScreen.Bounds.Width - 420) / 2, (Screen.PrimaryScreen.Bounds.Height - 520) / 2),
                BackColor = Theme.CardColor,
                BorderStyle = BorderStyle.FixedSingle
            };

            var header = new Panel
            {
                Size = new Size(420, 80),
                Location = new Point(0, 0),
                BackColor = Theme.PrimaryColor
            };

            lblTitle = new Label
            {
                Text = "LOGIN",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(140, 25)
            };

            lblIcon = new Label
            {
                Text = "🚗",
                Font = new Font("Segoe UI", 24F),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(180, 5)
            };

            header.Controls.Add(lblTitle);
            header.Controls.Add(lblIcon);

            lblUsername = new Label
            {
                Text = "Username",
                Font = Theme.HeadingFont,
                ForeColor = Theme.TextColor,
                AutoSize = true,
                Location = new Point(50, 110)
            };

            txtUsername = new TextBox
            {
                Width = 320,
                Location = new Point(50, 145),
                Font = Theme.BodyFont,
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblPassword = new Label
            {
                Text = "Password",
                Font = Theme.HeadingFont,
                ForeColor = Theme.TextColor,
                AutoSize = true,
                Location = new Point(50, 210)
            };

            txtPassword = new TextBox
            {
                Width = 320,
                Location = new Point(50, 245),
                Font = Theme.BodyFont,
                PasswordChar = '*',
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblStatus = new Label
            {
                ForeColor = Theme.DangerColor,
                Font = Theme.BodyFont,
                AutoSize = true,
                Location = new Point(50, 310),
                Height = 20
            };

            btnLogin = new Button
            {
                Text = "LOGIN",
                Width = 320,
                Location = new Point(50, 340),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            Theme.ApplyButtonStyle(btnLogin);
            btnLogin.Click += BtnLogin_Click;
            btnLogin.FlatAppearance.MouseOverBackColor = Theme.PrimaryLight;

            btnChangePassword = new Button
            {
                Text = "Change Password",
                Width = 150,
                Location = new Point(50, 400),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                ForeColor = Theme.PrimaryColor,
                Font = Theme.BodyFont,
                BackColor = Color.Transparent
            };
            btnChangePassword.FlatAppearance.BorderSize = 0;
            btnChangePassword.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            btnChangePassword.Click += BtnChangePassword_Click;

            loginPanel.Controls.Add(header);
            loginPanel.Controls.Add(lblUsername);
            loginPanel.Controls.Add(txtUsername);
            loginPanel.Controls.Add(lblPassword);
            loginPanel.Controls.Add(txtPassword);
            loginPanel.Controls.Add(lblStatus);
            loginPanel.Controls.Add(btnLogin);
            loginPanel.Controls.Add(btnChangePassword);
            Controls.Add(loginPanel);

            Text = "Login - Vehicle Rental";
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Theme.BackgroundColor;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var username = txtUsername.Text.Trim();
                var password = txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    lblStatus.Text = "Please enter username and password.";
                    return;
                }

                if (DatabaseHelper.ValidateUser(username, password, out _))
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    lblStatus.Text = "Invalid username or password.";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            using (var cp = new frmChangePassword(txtUsername.Text.Trim()))
            {
                cp.ShowDialog();
            }
        }
    }
}
