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
        private Label lblTitle;

        public frmManageVehicles() => InitializeComponent();

        private void InitializeComponent()
        {
            lblTitle = new Label
            {
                Text = "Manage Vehicles",
                Font = Theme.TitleFont,
                ForeColor = Theme.PrimaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
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

            txtSearch = new TextBox { Width = 300, Location = new Point(20, 60), Font = Theme.BodyFont, BorderStyle = BorderStyle.FixedSingle };
            txtSearch.TextChanged += (s, e) => LoadData();

            btnDelete = new Button { Text = "Delete Selected", Width = 140, Location = new Point(20, 60) };
            btnDelete.FlatStyle = FlatStyle.Flat;
            Theme.ApplyButtonStyle(btnDelete, Theme.DangerColor);
            btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 50, 60);
            btnDelete.Click += BtnDelete_Click;

            btnRefresh = new Button { Text = "Refresh", Width = 120, Location = new Point(170, 60) };
            btnRefresh.FlatStyle = FlatStyle.Flat;
            Theme.ApplyButtonStyle(btnRefresh, Theme.PrimaryColor);
            btnRefresh.FlatAppearance.MouseOverBackColor = Theme.PrimaryLight;
            btnRefresh.Click += (s, e) => LoadData();

            var topPanel = new Panel { Dock = DockStyle.Top, AutoSize = true };
            topPanel.Controls.Add(lblTitle);
            topPanel.Controls.Add(txtSearch);
            topPanel.Controls.Add(btnDelete);
            topPanel.Controls.Add(btnRefresh);

            Controls.Add(topPanel);
            Controls.Add(dgv);
            Text = "Manage Vehicles";
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            BackColor = Theme.BackgroundColor;
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
