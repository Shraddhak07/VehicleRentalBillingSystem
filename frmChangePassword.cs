using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmChangePassword : Form
    {
        private Label lblOld, lblNew, lblConfirm;
        private TextBox txtOld, txtNew, txtConfirm;
        private Button btnSave;
        public string Username { get; set; }

        public frmChangePassword()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            lblOld = new Label { Text = "Old Password:", AutoSize = true, Location = new Point(40, 30) };
            lblNew = new Label { Text = "New Password:", AutoSize = true, Location = new Point(40, 80) };
            lblConfirm = new Label { Text = "Confirm Password:", AutoSize = true, Location = new Point(40, 130) };

            txtOld = new TextBox { Width = 260, Location = new Point(160, 27), PasswordChar = '*' };
            txtNew = new TextBox { Width = 260, Location = new Point(160, 77), PasswordChar = '*' };
            txtConfirm = new TextBox { Width = 260, Location = new Point(160, 127), PasswordChar = '*' };

            btnSave = new Button { Text = "Save", Width = 120, Location = new Point(160, 165) };
            btnSave.Click += BtnSave_Click;

            Controls.AddRange(new Control[] { lblOld, lblNew, lblConfirm, txtOld, txtNew, txtConfirm, btnSave });
            Text = "Change Password";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            AcceptButton = btnSave;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNew.Text != txtConfirm.Text)
                {
                    MessageBox.Show("New passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DatabaseHelper.UpdatePassword(Username, txtNew.Text);
                MessageBox.Show("Password updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
