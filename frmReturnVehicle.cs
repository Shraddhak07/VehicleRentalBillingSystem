using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace VehicleRentalApp
{
    public partial class frmReturnVehicle : Form
    {
        private DataGridView dgv;
        private TextBox txtCustomer, txtVehicle, txtRentDate, txtPlannedReturn, txtPerDayRate, txtTotal, txtLateFee;
        private DateTimePicker dtpActualReturn;
        private Button btnSelect, btnReturn;

        public frmReturnVehicle() => InitializeComponent();

        private void InitializeComponent()
        {
            dgv = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 200,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgv.SelectionChanged += Dgv_SelectionChanged;

            var inputs = new TableLayoutPanel { ColumnCount = 2, RowCount = 8, Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(10) };
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));

            void AddRow(string label, Control control, int row)
            {
                inputs.Controls.Add(new Label { Text = label, AutoSize = true }, 0, row);
                inputs.Controls.Add(control, 1, row);
            }

            txtCustomer = new TextBox { Width = 300, ReadOnly = true }; AddRow("Customer:", txtCustomer, 0);
            txtVehicle = new TextBox { Width = 300, ReadOnly = true }; AddRow("Vehicle:", txtVehicle, 1);
            txtRentDate = new TextBox { Width = 300, ReadOnly = true }; AddRow("Rent Date:", txtRentDate, 2);
            txtPlannedReturn = new TextBox { Width = 300, ReadOnly = true }; AddRow("Planned Return:", txtPlannedReturn, 3);
            txtPerDayRate = new TextBox { Width = 300, ReadOnly = true }; AddRow("Per Day Rate:", txtPerDayRate, 4);
            dtpActualReturn = new DateTimePicker { Width = 300, Value = DateTime.Today }; AddRow("Actual Return:", dtpActualReturn, 5);
            txtLateFee = new TextBox { Width = 300, ReadOnly = true }; AddRow("Late Fee:", txtLateFee, 6);
            txtTotal = new TextBox { Width = 300, ReadOnly = true }; AddRow("Total:", txtTotal, 7);

            btnSelect = new Button { Text = "Select Rental", Width = 150 };
            btnReturn = new Button { Text = "Process Return", Width = 150 };
            btnReturn.Click += BtnReturn_Click;
            var btnPanel = new FlowLayoutPanel { AutoSize = true, Controls = { btnSelect, btnReturn } };
            inputs.Controls.Add(btnPanel, 1, 8);

            Controls.Add(dgv);
            Controls.Add(inputs);
            Text = "Return Vehicle";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            LoadRentals();
        }

        private void LoadRentals() => dgv.DataSource = DatabaseHelper.GetActiveRentals();

        private void Dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            var row = dgv.CurrentRow.DataBoundItem as DataRowView;
            txtCustomer.Text = row["CustomerName"].ToString();
            txtVehicle.Text = row["VehicleName"].ToString();
            txtRentDate.Text = Convert.ToDateTime(row["RentDate"]).ToShortDateString();
            txtPlannedReturn.Text = Convert.ToDateTime(row["ReturnDate"]).ToShortDateString();
            txtPerDayRate.Text = Convert.ToDecimal(row["PerDayRate"]).ToString("C");
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            try
            {
                var planned = Convert.ToDateTime(txtPlannedReturn.Text);
                var actual = dtpActualReturn.Value;
                var lateDays = (actual - planned).Days;
                var lateFee = lateDays > 0 ? lateDays * 50m : 0m;
                var rate = Convert.ToDecimal(txtPerDayRate.Text.Replace("$", "").Replace(",", ""));
                var rentDate = Convert.ToDateTime(txtRentDate.Text);
                var days = (actual - rentDate).Days;
                var total = days * rate + lateFee;
                txtLateFee.Text = lateFee.ToString("C");
                txtTotal.Text = total.ToString("C");
            }
            catch { txtTotal.Text = "0.00"; txtLateFee.Text = "0.00"; }
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            try
            {
                var rentalId = Convert.ToInt32(dgv.CurrentRow.Cells["RentalID"].Value);
                var vehicleId = Convert.ToInt32(dgv.CurrentRow.Cells["VehicleID"].Value);
                var perDayRate = Convert.ToDecimal(txtPerDayRate.Text.Replace("$", "").Replace(",", ""));
                var rentDate = Convert.ToDateTime(txtRentDate.Text);
                var actualReturn = dtpActualReturn.Value;
                var lateFee = (actualReturn - Convert.ToDateTime(txtPlannedReturn.Text)).Days > 0 ? (actualReturn - Convert.ToDateTime(txtPlannedReturn.Text)).Days * 50m : 0m;
                var total = (actualReturn - rentDate).Days * perDayRate + lateFee;

                OleDbConnection conn = null;
                OleDbTransaction txn = null;
                try
                {
                    conn = DatabaseHelper.GetOpenConnection();
                    txn = conn.BeginTransaction();

                    using (var cmd = new OleDbCommand("UPDATE Rentals SET ReturnDate=?, TotalAmount=?, PaymentStatus='Paid' WHERE RentalID=?", conn, txn))
                    {
                        cmd.Parameters.AddWithValue("?", actualReturn);
                        cmd.Parameters.AddWithValue("?", total);
                        cmd.Parameters.AddWithValue("?", rentalId);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new OleDbCommand("UPDATE Vehicles SET Status='Available' WHERE VehicleID=?", conn, txn))
                    {
                        cmd.Parameters.AddWithValue("?", vehicleId);
                        cmd.ExecuteNonQuery();
                    }

                    txn.Commit();
                    MessageBox.Show("Return processed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRentals();
                }
                catch (Exception) { txn?.Rollback(); throw; }
                finally { conn?.Close(); }
            }
            catch (Exception ex) { MessageBox.Show("Return failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
