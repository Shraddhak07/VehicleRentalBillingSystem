using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace VehicleRentalApp
{
    public partial class frmBookRental : Form
    {
        private DataTable dtCustomers, dtVehicles;
        private ComboBox cmbCustomer, cmbVehicle;
        private DateTimePicker dtpRent, dtpReturn;
        private TextBox txtRate, txtTotal;
        private DataGridView dgvSummary;
        private Button btnCalculate, btnBookNow;

        public frmBookRental() => InitializeComponent();

        private void InitializeComponent()
        {
            dtCustomers = DatabaseHelper.GetAllCustomers();
            dtVehicles = DatabaseHelper.GetAvailableVehicles();

            var tlp = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 7,
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10)
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));

            void AddLabelControl(string labelText, Control control, int row)
            {
                tlp.Controls.Add(new Label { Text = labelText, AutoSize = true }, 0, row);
                tlp.Controls.Add(control, 1, row);
            }

            cmbCustomer = new ComboBox { Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, DataSource = dtCustomers, DisplayMember = "Name", ValueMember = "CustomerID" };
            AddLabelControl("Customer:", cmbCustomer, 0);

            cmbVehicle = new ComboBox { Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, DataSource = dtVehicles, DisplayMember = "VehicleName", ValueMember = "VehicleID" };
            cmbVehicle.SelectedIndexChanged += CmbVehicle_SelectedIndexChanged;
            AddLabelControl("Vehicle:", cmbVehicle, 1);

            dtpRent = new DateTimePicker { Width = 300, Value = DateTime.Today };
            dtpRent.ValueChanged += (s, e) => CalculateTotal();
            AddLabelControl("Rent Date:", dtpRent, 2);

            dtpReturn = new DateTimePicker { Width = 300, Value = DateTime.Today.AddDays(3) };
            dtpReturn.ValueChanged += (s, e) => CalculateTotal();
            AddLabelControl("Return Date:", dtpReturn, 3);

            txtRate = new TextBox { Width = 300, ReadOnly = true };
            AddLabelControl("Per Day Rate:", txtRate, 4);

            txtTotal = new TextBox { Width = 300, ReadOnly = true };
            AddLabelControl("Total Amount:", txtTotal, 5);

            btnCalculate = new Button { Text = "Calculate", Width = 120 };
            btnCalculate.Click += (s, e) => CalculateTotal();
            btnBookNow = new Button { Text = "Book Rental", Width = 150 };
            btnBookNow.Click += BtnBookNow_Click;
            var btnPanel = new FlowLayoutPanel { AutoSize = true, Controls = { btnCalculate, btnBookNow } };
            tlp.Controls.Add(btnPanel, 1, 6);

            dgvSummary = new DataGridView { ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, Width = 620 };
            LoadSummary();

            Controls.Add(tlp);
            Controls.Add(dgvSummary);
            Text = "Book Rental";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            AcceptButton = btnBookNow;
        }

        private void CmbVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVehicle.SelectedItem is DataRowView row)
                txtRate.Text = Convert.ToDecimal(row["PerDayRate"]).ToString("C");
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            try
            {
                var rate = Convert.ToDecimal(cmbVehicle.SelectedItem is DataRowView row ? row["PerDayRate"] : 0);
                var days = (dtpReturn.Value - dtpRent.Value).Days;
                txtTotal.Text = days > 0 ? (days * rate).ToString("C") : "0.00";
            }
            catch { txtTotal.Text = "0.00"; }
        }

        private void LoadSummary()
        {
            var dt = new DataTable();
            dt.Columns.Add("Field", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Rows.Add("Customer", cmbCustomer.Text);
            dt.Rows.Add("Vehicle", cmbVehicle.Text);
            dt.Rows.Add("Rent Date", dtpRent.Value.ToShortDateString());
            dt.Rows.Add("Return Date", dtpReturn.Value.ToShortDateString());
            dt.Rows.Add("Total Amount", txtTotal.Text);
            dgvSummary.DataSource = dt;
        }

        private void BtnBookNow_Click(object sender, EventArgs e)
        {
            try
            {
                var customerId = Convert.ToInt32(cmbCustomer.SelectedValue);
                var vehicleId = Convert.ToInt32(cmbVehicle.SelectedValue);
                var rate = Convert.ToDecimal(cmbVehicle.SelectedItem is DataRowView row ? row["PerDayRate"] : 0);
                var days = (dtpReturn.Value - dtpRent.Value).Days;
                var total = days * rate;

                OleDbConnection conn = null;
                OleDbTransaction txn = null;
                try
                {
                    conn = DatabaseHelper.GetOpenConnection();
                    txn = conn.BeginTransaction();

                    using (var cmd = new OleDbCommand(
                        "INSERT INTO Rentals (CustomerID, VehicleID, RentDate, ReturnDate, TotalAmount, PaymentStatus) VALUES (?, ?, ?, ?, ?, 'Pending')", conn, txn))
                    {
                        cmd.Parameters.AddWithValue("?", customerId);
                        cmd.Parameters.AddWithValue("?", vehicleId);
                        cmd.Parameters.AddWithValue("?", dtpRent.Value);
                        cmd.Parameters.AddWithValue("?", dtpReturn.Value);
                        cmd.Parameters.AddWithValue("?", total);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new OleDbCommand(
                        "UPDATE Vehicles SET Status='Rented' WHERE VehicleID=? AND Status='Available'", conn, txn))
                    {
                        cmd.Parameters.AddWithValue("?", vehicleId);
                        if (cmd.ExecuteNonQuery() == 0)
                            throw new Exception("Vehicle is no longer available.");
                    }

                    txn.Commit();
                    MessageBox.Show($"Rental booked successfully. Total: {total:C}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSummary();
                }
                catch (Exception)
                {
                    txn?.Rollback();
                    throw;
                }
                finally { conn?.Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Booking failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
