using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmVehicleAvailability : Form
    {
        private DataGridView dgv;
        private Button btnRefresh;

        public frmVehicleAvailability() => InitializeComponent();

        private void InitializeComponent()
        {
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            btnRefresh = new Button { Text = "Refresh", Width = 120, Location = new Point(10, 10) };
            btnRefresh.Click += (s, e) => LoadAvailability();
            var topPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Controls = { btnRefresh } };
            Controls.Add(topPanel);
            Controls.Add(dgv);
            Text = "Vehicle Availability";
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            LoadAvailability();
        }

        private void LoadAvailability() => dgv.DataSource = DatabaseHelper.GetVehicleAvailability();
    }
}
