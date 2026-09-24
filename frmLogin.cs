using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmLogin : Form
    {
        private Label lblUsername, lblPassword, lblStatus;
        private TextBox txtUsername, txtPassword;
        private Button btnLogin, btnChangePassword;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            lblUsername = new Label { Text = "Username:", AutoSize = true, Location = new Point(40, 40) };
            lblPassword = new Label { Text = "Password:", AutoSize = true, Location = new Point(40, 90) };
            lblStatus = new Label { ForeColor = Color.Red, AutoSize = true, Location = new Point(40, 160) };

            txtUsername = new TextBox { Width = 260, Location = new Point(140, 37) };
            txtPassword = new TextBox { Width = 260, Location = new Point(140, 87), PasswordChar = '*' };

            btnLogin = new Button { Text = "Login", Width = 120, Location = new Point(140, 120) };
            btnLogin.Click += BtnLogin_Click;

            btnChangePassword = new Button { Text = "Change Password", Width = 140, Location = new Point(270, 120), FlatStyle = FlatStyle.Flat };
            btnChangePassword.Click += BtnChangePassword_Click;

            Controls.AddRange(new Control[] { lblUsername, lblPassword, lblStatus, txtUsername, txtPassword, btnLogin, btnChangePassword });
            Text = "Staff Login";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            AcceptButton = btnLogin;
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
                    Program.CurrentUsername = username;
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
