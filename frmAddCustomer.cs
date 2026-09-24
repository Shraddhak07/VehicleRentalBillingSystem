using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace VehicleRentalApp
{
    public partial class frmAddCustomer : Form
    {
        private TextBox txtName, txtPhone, txtLicence;
        private Button btnAdd;
        private Label lblTitle;

        public frmAddCustomer() => InitializeComponent();

        private void InitializeComponent()
        {
            lblTitle = new Label
            {
                Text = "Add Customer",
                Font = Theme.TitleFont,
                ForeColor = Theme.PrimaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            var formPanel = new Panel
            {
                Size = new Size(500, 400),
                Location = new Point((Screen.PrimaryScreen.Bounds.Width - 500) / 2, (Screen.PrimaryScreen.Bounds.Height - 400) / 2),
                BackColor = Theme.CardColor,
                BorderStyle = BorderStyle.FixedSingle
            };

            void AddInput(Label label, TextBox textBox, int y)
            {
                label.ForeColor = Theme.TextColor;
                label.Font = Theme.HeadingFont;
                label.AutoSize = true;
                label.Location = new Point(40, y);
                textBox.Width = 400;
                textBox.Location = new Point(40, y + 35);
                textBox.Font = Theme.BodyFont;
                textBox.BorderStyle = BorderStyle.FixedSingle;
                formPanel.Controls.Add(label);
                formPanel.Controls.Add(textBox);
            }

            txtName = new TextBox();
            txtPhone = new TextBox();
            txtLicence = new TextBox();

            AddInput(new Label { Text = "Name:" }, txtName, 80);
            AddInput(new Label { Text = "Phone:" }, txtPhone, 155);
            AddInput(new Label { Text = "Licence Number:" }, txtLicence, 230);

            btnAdd = new Button { Text = "Add Customer", Width = 200, Location = new Point(150, 350) };
            Theme.ApplyButtonStyle(btnAdd, Theme.SuccessColor);
            btnAdd.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 180, 90);
            btnAdd.Click += BtnAdd_Click;
            formPanel.Controls.Add(btnAdd);

            Controls.Add(formPanel);
            Controls.Add(lblTitle);
            Text = "Add Customer - Vehicle Rental";
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Theme.BackgroundColor;
            WindowState = FormWindowState.Maximized;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using var conn = DatabaseHelper.GetOpenConnection();
                using var cmd = new OleDbCommand(
                    "INSERT INTO Customers (Name, Phone, LicenceNumber) VALUES (?, ?, ?)", conn);
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", txtLicence.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Clear(); txtPhone.Clear(); txtLicence.Clear();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
