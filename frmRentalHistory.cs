using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmRentalHistory : Form
    {
        private DataGridView dgv;

        public frmRentalHistory() => InitializeComponent();

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
            Controls.Add(dgv);
            Text = "Rental History";
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            LoadHistory();
        }

        private void LoadHistory() => dgv.DataSource = DatabaseHelper.GetRentalHistory();
    }
}
