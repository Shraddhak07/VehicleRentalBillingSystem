using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace VehicleRentalApp
{
    public partial class frmManageVehicles : Form
    {
        private DataGridView dgv;
        private TextBox txtSearch;
        private Button btnDelete, btnRefresh;

        public frmManageVehicles() => InitializeComponent();

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

            txtSearch = new TextBox { Width = 300, Location = new Point(10, 10) };
            txtSearch.TextChanged += (s, e) => LoadData();

            btnDelete = new Button { Text = "Delete", Width = 120, Location = new Point(10, 40) };
            btnDelete.Click += BtnDelete_Click;

            btnRefresh = new Button { Text = "Refresh", Width = 120, Location = new Point(140, 40) };
            btnRefresh.Click += (s, e) => LoadData();

            var topPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Controls = { txtSearch, btnDelete, btnRefresh } };
            Controls.Add(topPanel);
            Controls.Add(dgv);
            Text = "Manage Vehicles";
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            LoadData();
        }

        private void LoadData() => dgv.DataSource = DatabaseHelper.GetAllVehicles();

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            if (dgv.CurrentRow.Cells["Status"].Value?.ToString() == "Rented")
            {
                MessageBox.Show("Cannot delete a rented vehicle.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var vehicleId = Convert.ToInt32(dgv.CurrentRow.Cells["VehicleID"].Value);
            try
            {
                DatabaseHelper.ExecuteNonQuery($"DELETE FROM Vehicles WHERE VehicleID={vehicleId}");
                LoadData();
                MessageBox.Show("Vehicle deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
