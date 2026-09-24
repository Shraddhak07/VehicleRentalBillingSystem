using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmRentalHistory : Form
    {
        private DataGridView dgv;
        private Label lblTitle;
        private Button btnRefresh;

        public frmRentalHistory() => InitializeComponent();

        private void InitializeComponent()
        {
            lblTitle = new Label
            {
                Text = "Rental History",
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

            btnRefresh = new Button { Text = "Refresh", Width = 120, Location = new Point(20, 60) };
            btnRefresh.FlatStyle = FlatStyle.Flat;
            Theme.ApplyButtonStyle(btnRefresh, Theme.PrimaryColor);
            btnRefresh.FlatAppearance.MouseOverBackColor = Theme.PrimaryLight;
            btnRefresh.Click += (s, e) => LoadHistory();

            var topPanel = new Panel { Dock = DockStyle.Top, AutoSize = true };
            topPanel.Controls.Add(lblTitle);
            topPanel.Controls.Add(btnRefresh);

            Controls.Add(topPanel);
            Controls.Add(dgv);
            Text = "Rental History";
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            BackColor = Theme.BackgroundColor;
            LoadHistory();
        }

        private void LoadHistory() => dgv.DataSource = DatabaseHelper.GetRentalHistory();
    }
}
