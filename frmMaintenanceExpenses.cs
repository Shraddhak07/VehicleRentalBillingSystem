using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace VehicleRentalApp
{
    public partial class frmMaintenanceExpenses : Form
    {
        private ComboBox cmbVehicle;
        private DateTimePicker dtpDate;
        private TextBox txtAmount, txtDescription;
        private Button btnSave;

        public frmMaintenanceExpenses() => InitializeComponent();

        private void InitializeComponent()
        {
            cmbVehicle = new ComboBox
            {
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = DatabaseHelper.GetAllVehicles(),
                DisplayMember = "VehicleName",
                ValueMember = "VehicleID"
            };
            dtpDate = new DateTimePicker { Width = 300, Value = DateTime.Today };
            txtAmount = new TextBox { Width = 300 };
            txtDescription = new TextBox { Width = 300 };
            btnSave = new Button { Text = "Save Expense", Width = 150 };
            btnSave.Click += BtnSave_Click;

            var tlp = new TableLayoutPanel { ColumnCount = 2, RowCount = 5, Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(10) };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));
            tlp.Controls.Add(new Label { Text = "Vehicle:", AutoSize = true }, 0, 0);
            tlp.Controls.Add(cmbVehicle, 1, 0);
            tlp.Controls.Add(new Label { Text = "Expense Date:", AutoSize = true }, 0, 1);
            tlp.Controls.Add(dtpDate, 1, 1);
            tlp.Controls.Add(new Label { Text = "Amount Spent:", AutoSize = true }, 0, 2);
            tlp.Controls.Add(txtAmount, 1, 2);
            tlp.Controls.Add(new Label { Text = "Description:", AutoSize = true }, 0, 3);
            tlp.Controls.Add(txtDescription, 1, 3);
            tlp.Controls.Add(btnSave, 1, 4);

            Controls.Add(tlp);
            Text = "Maintenance Expenses";
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
                using var conn = DatabaseHelper.GetOpenConnection();
                using var cmd = new OleDbCommand(
                    "INSERT INTO Maintenance (VehicleID, ExpenseDate, AmountSpent, Description) VALUES (?, ?, ?, ?)", conn);
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(cmbVehicle.SelectedValue));
                cmd.Parameters.AddWithValue("?", dtpDate.Value);
                cmd.Parameters.AddWithValue("?", decimal.Parse(txtAmount.Text));
                cmd.Parameters.AddWithValue("?", txtDescription.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Maintenance expense recorded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
