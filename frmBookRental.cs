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
        private Label lblTitle;

        public frmBookRental() => InitializeComponent();

        private void InitializeComponent()
        {
            lblTitle = new Label
            {
                Text = "Book Rental",
                Font = Theme.TitleFont,
                ForeColor = Theme.PrimaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            dtCustomers = DatabaseHelper.GetAllCustomers();
            dtVehicles = DatabaseHelper.GetAvailableVehicles();

            var formPanel = new Panel
            {
                Size = new Size(700, 500),
                Location = new Point((Screen.PrimaryScreen.Bounds.Width - 700) / 2, (Screen.PrimaryScreen.Bounds.Height - 500) / 2),
                BackColor = Theme.CardColor,
                BorderStyle = BorderStyle.FixedSingle
            };

            void AddInput(Label label, Control control, int y)
            {
                label.ForeColor = Theme.TextColor;
                label.Font = Theme.HeadingFont;
                label.AutoSize = true;
                label.Location = new Point(40, y);
                control.Location = new Point(40, y + 35);
                formPanel.Controls.Add(label);
                formPanel.Controls.Add(control);
            }

            cmbCustomer = new ComboBox { Width = 400, DropDownStyle = ComboBoxStyle.DropDownList, DataSource = dtCustomers, DisplayMember = "Name", ValueMember = "CustomerID", Font = Theme.BodyFont };
            cmbVehicle = new ComboBox { Width = 400, DropDownStyle = ComboBoxStyle.DropDownList, DataSource = dtVehicles, DisplayMember = "VehicleName", ValueMember = "VehicleID", Font = Theme.BodyFont };
            cmbVehicle.SelectedIndexChanged += CmbVehicle_SelectedIndexChanged;

            dtpRent = new DateTimePicker { Width = 400, Value = DateTime.Today, Font = Theme.BodyFont };
            dtpReturn = new DateTimePicker { Width = 400, Value = DateTime.Today.AddDays(3), Font = Theme.BodyFont };
            dtpReturn.ValueChanged += (s, e) => CalculateTotal();

            txtRate = new TextBox { Width = 400, ReadOnly = true, Font = Theme.BodyFont, BorderStyle = BorderStyle.FixedSingle };
            txtTotal = new TextBox { Width = 400, ReadOnly = true, Font = Theme.BodyFont, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(240, 255, 240) };

            AddInput(new Label { Text = "Customer:" }, cmbCustomer, 80);
            AddInput(new Label { Text = "Vehicle:" }, cmbVehicle, 140);
            AddInput(new Label { Text = "Rent Date:" }, dtpRent, 200);
            AddInput(new Label { Text = "Return Date:" }, dtpReturn, 260);
            AddInput(new Label { Text = "Per Day Rate:" }, txtRate, 320);
            AddInput(new Label { Text = "Total Amount:" }, txtTotal, 380);

            btnCalculate = new Button { Text = "Calculate", Width = 140, Location = new Point(40, 430) };
            btnCalculate.FlatStyle = FlatStyle.Flat;
            Theme.ApplyButtonStyle(btnCalculate, Theme.PrimaryLight);
            btnCalculate.FlatAppearance.MouseOverBackColor = Theme.PrimaryColor;
            btnCalculate.Click += (s, e) => CalculateTotal();

            btnBookNow = new Button { Text = "Book Rental", Width = 180, Location = new Point(200, 430) };
            btnBookNow.FlatStyle = FlatStyle.Flat;
            Theme.ApplyButtonStyle(btnBookNow, Theme.SuccessColor);
            btnBookNow.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 180, 90);
            btnBookNow.Click += BtnBookNow_Click;

            dgvSummary = new DataGridView { ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, Width = 620, Location = new Point(40, 480), Height = 0, BorderStyle = BorderStyle.FixedSingle };
            LoadSummary();

            formPanel.Controls.Add(btnCalculate);
            formPanel.Controls.Add(btnBookNow);
            formPanel.Controls.Add(dgvSummary);

            Controls.Add(formPanel);
            Controls.Add(lblTitle);
            Text = "Book Rental - Vehicle Rental";
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Theme.BackgroundColor;
            WindowState = FormWindowState.Maximized;
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
                var rate = cmbVehicle.SelectedItem is DataRowView row ? Convert.ToDecimal(row["PerDayRate"]) : 0m;
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
                var rate = cmbVehicle.SelectedItem is DataRowView row ? Convert.ToDecimal(row["PerDayRate"]) : 0m;
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
                    MessageBox.Show($"Rental booked! Total: {total:C}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSummary();
                }
                catch (Exception) { txn?.Rollback(); throw; }
                finally { conn?.Close(); }
            }
            catch (Exception ex) { MessageBox.Show("Booking failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
