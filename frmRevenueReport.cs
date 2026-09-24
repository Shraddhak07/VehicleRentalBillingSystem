using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmRevenueReport : Form
    {
        private DateTimePicker dtpStart, dtpEnd;
        private DataGridView dgv;
        private Label lblTotal, lblTitle;
        private Button btnGenerate;

        public frmRevenueReport() => InitializeComponent();

        private void InitializeComponent()
        {
            lblTitle = new Label
            {
                Text = "Revenue Report",
                Font = Theme.TitleFont,
                ForeColor = Theme.PrimaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            dtpStart = new DateTimePicker { Width = 200, Value = DateTime.Today.AddDays(-7), Font = Theme.BodyFont, Location = new Point(20, 65) };
            dtpEnd = new DateTimePicker { Width = 200, Value = DateTime.Today, Font = Theme.BodyFont, Location = new Point(230, 65) };
            btnGenerate = new Button { Text = "Generate Report", Width = 150, Location = new Point(440, 62) };
            btnGenerate.FlatStyle = FlatStyle.Flat;
            Theme.ApplyButtonStyle(btnGenerate, Theme.SuccessColor);
            btnGenerate.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 180, 90);
            btnGenerate.Click += BtnGenerate_Click;

            lblTotal = new Label
            {
                AutoSize = true,
                Text = "Total Revenue: $0.00",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Theme.SuccessColor,
                Location = new Point(20, 110)
            };

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
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

            var topPanel = new Panel { Dock = DockStyle.Top, AutoSize = true };
            topPanel.Controls.Add(lblTitle);
            topPanel.Controls.Add(dtpStart);
            topPanel.Controls.Add(dtpEnd);
            topPanel.Controls.Add(btnGenerate);

            Controls.Add(topPanel);
            Controls.Add(lblTotal);
            Controls.Add(dgv);
            Text = "Revenue Report";
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            BackColor = Theme.BackgroundColor;
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                var dt = DatabaseHelper.GetRevenueReport(dtpStart.Value, dtpEnd.Value);
                dgv.DataSource = dt;
                var sum = dt.Compute("SUM(TotalAmount)", "");
                lblTotal.Text = $"Total Revenue: {sum}";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
