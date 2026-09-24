using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmExpenseReport : Form
    {
        private DateTimePicker dtpStart, dtpEnd;
        private DataGridView dgv;
        private Label lblTotal;
        private Button btnGenerate;

        public frmExpenseReport() => InitializeComponent();

        private void InitializeComponent()
        {
            dtpStart = new DateTimePicker { Width = 200, Value = DateTime.Today.AddDays(-7) };
            dtpEnd = new DateTimePicker { Width = 200, Value = DateTime.Today };
            btnGenerate = new Button { Text = "Generate Report", Width = 150 };
            btnGenerate.Click += BtnGenerate_Click;
            lblTotal = new Label { AutoSize = true, Text = "Total Expenses: 0.00", Font = new System.Drawing.Font("Segoe UI", 12F) };
            dgv = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            var topPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Controls = { dtpStart, dtpEnd, btnGenerate } };
            Controls.Add(topPanel);
            Controls.Add(lblTotal);
            Controls.Add(dgv);
            Text = "Expense Report";
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                var dt = DatabaseHelper.GetExpenseReport(dtpStart.Value, dtpEnd.Value);
                dgv.DataSource = dt;
                var sum = dt.Compute("SUM(AmountSpent)", "");
                lblTotal.Text = $"Total Expenses: {sum}";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
