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

        public frmAddCustomer() => InitializeComponent();

        private void InitializeComponent()
        {
            var tlp = new TableLayoutPanel { ColumnCount = 2, RowCount = 4, Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(10) };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));

            tlp.Controls.Add(new Label { Text = "Name:", AutoSize = true }, 0, 0);
            txtName = new TextBox { Width = 300 };
            tlp.Controls.Add(txtName, 1, 0);

            tlp.Controls.Add(new Label { Text = "Phone:", AutoSize = true }, 0, 1);
            txtPhone = new TextBox { Width = 300 };
            tlp.Controls.Add(txtPhone, 1, 1);

            tlp.Controls.Add(new Label { Text = "Licence Number:", AutoSize = true }, 0, 2);
            txtLicence = new TextBox { Width = 300 };
            tlp.Controls.Add(txtLicence, 1, 2);

            btnAdd = new Button { Text = "Add Customer", Width = 150 };
            btnAdd.Click += BtnAdd_Click;
            tlp.Controls.Add(btnAdd, 1, 3);

            Controls.Add(tlp);
            Text = "Add Customer";
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
                    "INSERT INTO Customers (Name, Phone, LicenceNumber) VALUES (?, ?, ?)", conn);
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", txtLicence.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Customer added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
