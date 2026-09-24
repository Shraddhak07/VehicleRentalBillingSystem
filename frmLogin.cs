using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmLogin : Form
    {
        private Label lblUsername, lblPassword, lblStatus;
        private TextBox txtUsername, txtPassword;
        private Button btnLogin;

        public frmLogin() => InitializeComponent();

        private void InitializeComponent()
        {
            lblUsername = new Label { Text = "Username:", AutoSize = true, Location = new Point(50, 50) };
            txtUsername = new TextBox { Width = 300, Location = new Point(50, 80) };
            lblPassword = new Label { Text = "Password:", AutoSize = true, Location = new Point(50, 130) };
            txtPassword = new TextBox { Width = 300, Location = new Point(50, 160), PasswordChar = '*' };
            lblStatus = new Label { ForeColor = Color.Red, AutoSize = true, Location = new Point(50, 210) };

            btnLogin = new Button { Text = "Login", Width = 120, Location = new Point(50, 240) };
            btnLogin.Click += BtnLogin_Click;

            Controls.AddRange(new Control[] { lblUsername, txtUsername, lblPassword, txtPassword, lblStatus, btnLogin });

            Text = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
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
    }
}
