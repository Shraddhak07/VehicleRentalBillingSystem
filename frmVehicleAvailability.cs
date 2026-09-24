using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmVehicleAvailability : Form
    {
        private DataGridView dgv;
        private Label lblTitle;
        private Button btnRefresh;
        private Label lblAvailable, lblRented;

        public frmVehicleAvailability() => InitializeComponent();

        private void InitializeComponent()
        {
            lblTitle = new Label
            {
                Text = "Vehicle Availability",
                Font = Theme.TitleFont,
                ForeColor = Theme.PrimaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblAvailable = new Label
            {
                Text = "Available: 0",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Theme.SuccessColor,
                AutoSize = true,
                Location = new Point(20, 65)
            };

            lblRented = new Label
            {
                Text = "Rented: 0",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Theme.DangerColor,
                AutoSize = true,
                Location = new Point(200, 65)
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

            btnRefresh = new Button { Text = "Refresh", Width = 120, Location = new Point(400, 62) };
            btnRefresh.FlatStyle = FlatStyle.Flat;
            Theme.ApplyButtonStyle(btnRefresh, Theme.PrimaryColor);
            btnRefresh.FlatAppearance.MouseOverBackColor = Theme.PrimaryLight;
            btnRefresh.Click += (s, e) => LoadAvailability();

            var topPanel = new Panel { Dock = DockStyle.Top, AutoSize = true };
            topPanel.Controls.Add(lblTitle);
            topPanel.Controls.Add(lblAvailable);
            topPanel.Controls.Add(lblRented);
            topPanel.Controls.Add(btnRefresh);

            Controls.Add(topPanel);
            Controls.Add(dgv);
            Text = "Vehicle Availability";
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            BackColor = Theme.BackgroundColor;
            LoadAvailability();
        }

        private void LoadAvailability()
        {
            var dt = DatabaseHelper.GetVehicleAvailability();
            dgv.DataSource = dt;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["Status"].Value?.ToString() == "Available")
                    lblAvailable.Text = $"Available: {row.Cells["Count"].Value}";
                else
                    lblRented.Text = $"Rented: {row.Cells["Count"].Value}";
            }
        }
    }
}
