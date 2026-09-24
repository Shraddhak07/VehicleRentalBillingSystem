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

        public frmAddVehicle() => InitializeComponent();

        private void InitializeComponent()
        {
            var tlp = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 5,
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10)
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));

            tlp.Controls.Add(new Label { Text = "Vehicle Name:", AutoSize = true }, 0, 0);
            txtName = new TextBox { Width = 300 };
            tlp.Controls.Add(txtName, 1, 0);

            tlp.Controls.Add(new Label { Text = "Category:", AutoSize = true }, 0, 1);
            txtCategory = new TextBox { Width = 300 };
            tlp.Controls.Add(txtCategory, 1, 1);

            tlp.Controls.Add(new Label { Text = "Per Day Rate:", AutoSize = true }, 0, 2);
            txtRate = new TextBox { Width = 300 };
            tlp.Controls.Add(txtRate, 1, 2);

            tlp.Controls.Add(new Label { Text = "Status:", AutoSize = true }, 0, 3);
            txtStatus = new TextBox { Width = 300, Text = "Available" };
            tlp.Controls.Add(txtStatus, 1, 3);

            btnAdd = new Button { Text = "Add Vehicle", Width = 150 };
            btnAdd.Click += BtnAdd_Click;
            tlp.Controls.Add(btnAdd, 1, 4);

            Controls.Add(tlp);
            Text = "Add Vehicle";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            AcceptButton = btnAdd;
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
                MessageBox.Show("Vehicle added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
