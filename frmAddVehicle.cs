using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace VehicleRentalApp
{
    public partial class frmAddVehicle : Form
    {
        private TextBox txtName, txtCategory, txtRate, txtStatus;
        private Button btnAdd;
        private Label lblTitle;

        public frmAddVehicle() => InitializeComponent();

        private void InitializeComponent()
        {
            lblTitle = new Label
            {
                Text = "Add Vehicle",
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
            txtCategory = new TextBox();
            txtRate = new TextBox();
            txtStatus = new TextBox { Text = "Available" };

            AddInput(new Label { Text = "Vehicle Name:" }, txtName, 80);
            AddInput(new Label { Text = "Category:" }, txtCategory, 155);
            AddInput(new Label { Text = "Per Day Rate:" }, txtRate, 230);
            AddInput(new Label { Text = "Status:" }, txtStatus, 305);

            btnAdd = new Button { Text = "Add Vehicle", Width = 200, Location = new Point(150, 350) };
            Theme.ApplyButtonStyle(btnAdd);
            btnAdd.FlatAppearance.MouseOverBackColor = Theme.PrimaryLight;
            btnAdd.Click += BtnAdd_Click;
            formPanel.Controls.Add(btnAdd);

            Controls.Add(formPanel);
            Controls.Add(lblTitle);
            Text = "Add Vehicle - Vehicle Rental";
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
                    "INSERT INTO Vehicles (VehicleName, Category, PerDayRate, Status) VALUES (?, ?, ?, ?)", conn);
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", txtCategory.Text);
                cmd.Parameters.AddWithValue("?", decimal.Parse(txtRate.Text));
                cmd.Parameters.AddWithValue("?", txtStatus.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Vehicle added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Clear(); txtCategory.Clear(); txtRate.Clear(); txtStatus.Text = "Available";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
