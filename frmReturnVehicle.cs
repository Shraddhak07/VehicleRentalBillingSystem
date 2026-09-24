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
        private Label lblTitle;

        public frmReturnVehicle() => InitializeComponent();

        private void InitializeComponent()
        {
            lblTitle = new Label
            {
                Text = "Return Vehicle",
                Font = Theme.TitleFont,
                ForeColor = Theme.PrimaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            dgv = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 180,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Theme.CardColor,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(245, 247, 250) },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Theme.PrimaryColor, ForeColor = Color.White, Font = new Font("Segoe UI", 10F, FontStyle.Bold) }
            };
            dgv.SelectionChanged += Dgv_SelectionChanged;

            var inputs = new Panel
            {
                Size = new Size(600, 420),
                Location = new Point(20, 200),
                BackColor = Theme.CardColor,
                BorderStyle = BorderStyle.FixedSingle
            };

            void AddRow(string label, Control control, int y)
            {
                var lbl = new Label { Text = label, AutoSize = true, ForeColor = Theme.TextColor, Font = Theme.HeadingFont, Location = new Point(40, y) };
                control.Location = new Point(40, y + 35);
                inputs.Controls.Add(lbl);
                inputs.Controls.Add(control);
            }

            txtCustomer = new TextBox { Width = 500, ReadOnly = true, Font = Theme.BodyFont, BorderStyle = BorderStyle.FixedSingle };
            txtVehicle = new TextBox { Width = 500, ReadOnly = true, Font = Theme.BodyFont, BorderStyle = BorderStyle.FixedSingle };
            txtRentDate = new TextBox { Width = 500, ReadOnly = true, Font = Theme.BodyFont, BorderStyle = BorderStyle.FixedSingle };
            txtPlannedReturn = new TextBox { Width = 500, ReadOnly = true, Font = Theme.BodyFont, BorderStyle = BorderStyle.FixedSingle };
            txtPerDayRate = new TextBox { Width = 500, ReadOnly = true, Font = Theme.BodyFont, BorderStyle = BorderStyle.FixedSingle };
            dtpActualReturn = new DateTimePicker { Width = 500, Value = DateTime.Today, Font = Theme.BodyFont };
            txtLateFee = new TextBox { Width = 500, ReadOnly = true, Font = Theme.BodyFont, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(255, 250, 240) };
            txtTotal = new TextBox { Width = 500, ReadOnly = true, Font = Theme.BodyFont, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(240, 255, 240) };

            AddRow("Customer:", txtCustomer, 30);
            AddRow("Vehicle:", txtVehicle, 85);
            AddRow("Rent Date:", txtRentDate, 140);
            AddRow("Planned Return:", txtPlannedReturn, 195);
            AddRow("Per Day Rate:", txtPerDayRate, 250);
            AddRow("Actual Return:", dtpActualReturn, 305);
            AddRow("Late Fee:", txtLateFee, 360);
            AddRow("Total:", txtTotal, 415);

            btnSelect = new Button { Text = "Select Rental", Width = 160, Location = new Point(40, 470) };
            btnSelect.FlatStyle = FlatStyle.Flat;
            Theme.ApplyButtonStyle(btnSelect, Theme.PrimaryColor);
            btnSelect.FlatAppearance.MouseOverBackColor = Theme.PrimaryLight;

            btnReturn = new Button { Text = "Process Return", Width = 180, Location = new Point(220, 470) };
            btnReturn.FlatStyle = FlatStyle.Flat;
            Theme.ApplyButtonStyle(btnReturn, Theme.DangerColor);
            btnReturn.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 50, 60);
            btnReturn.Click += BtnReturn_Click;

            var btnPanel = new Panel { Dock = DockStyle.Top, AutoSize = true };
            btnPanel.Controls.Add(btnSelect);
            btnPanel.Controls.Add(btnReturn);
            inputs.Controls.Add(btnPanel);

            Controls.Add(lblTitle);
            Controls.Add(dgv);
            Controls.Add(inputs);
            Text = "Return Vehicle - Vehicle Rental";
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Theme.BackgroundColor;
            WindowState = FormWindowState.Maximized;
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
                    MessageBox.Show("Return processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRentals();
                }
                catch (Exception) { txn?.Rollback(); throw; }
                finally { conn?.Close(); }
            }
            catch (Exception ex) { MessageBox.Show("Return failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
