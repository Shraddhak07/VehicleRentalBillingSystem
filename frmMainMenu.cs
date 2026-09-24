using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmMainMenu : Form
    {
        public frmMainMenu() => InitializeComponent();

        private void InitializeComponent()
        {
            IsMdiContainer = true;
            Text = "Vehicle Rental & Billing System";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.Manual;
            BackColor = Theme.BackgroundColor;

            var menuStrip = new MenuStrip
            {
                RenderMode = ToolStripRenderMode.Professional,
                BackColor = Theme.PrimaryColor,
                ForeColor = Color.White,
                Height = 40
            };

            var fileMenu = new ToolStripDropDownButton("File");
            fileMenu.ForeColor = Color.White;
            fileMenu.Click += (s, e) => Application.Exit();
            menuStrip.Items.Add(fileMenu);

            var helpMenu = new ToolStripDropDownButton("Help");
            helpMenu.ForeColor = Color.White;
            helpMenu.Click += (s, e) => { var f = new frmAbout(); f.ShowDialog(); };
            menuStrip.Items.Add(helpMenu);

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(15, 15, 15, 15),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            void AddCard(string text, Color color, EventHandler handler)
            {
                var btn = new Button
                {
                    Text = text,
                    AutoSize = true,
                    Margin = new Padding(8),
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    BackColor = color,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Padding = new Padding(20, 12, 20, 12)
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Theme.PrimaryLight;
                btn.Click += handler;
                panel.Controls.Add(btn);
            }

            AddCard("Add Vehicle", Theme.PrimaryColor, (s, e) => { var f = new frmAddVehicle(); f.MdiParent = this; f.Show(); });
            AddCard("Manage Vehicles", Theme.PrimaryDark, (s, e) => { var f = new frmManageVehicles(); f.MdiParent = this; f.Show(); });
            AddCard("Add Customer", Theme.SuccessColor, (s, e) => { var f = new frmAddCustomer(); f.MdiParent = this; f.Show(); });
            AddCard("Manage Customers", Theme.PrimaryColor, (s, e) => { var f = new frmManageCustomers(); f.MdiParent = this; f.Show(); });
            AddCard("Book Rental", Theme.AccentColor, (s, e) => { var f = new frmBookRental(); f.MdiParent = this; f.Show(); });
            AddCard("Return Vehicle", Theme.DangerColor, (s, e) => { var f = new frmReturnVehicle(); f.MdiParent = this; f.Show(); });
            AddCard("Maintenance", Theme.WarningColor, (s, e) => { var f = new frmMaintenanceExpenses(); f.MdiParent = this; f.Show(); });
            AddCard("Rental History", Theme.PrimaryLight, (s, e) => { var f = new frmRentalHistory(); f.MdiParent = this; f.Show(); });
            AddCard("Revenue Report", Theme.SuccessColor, (s, e) => { var f = new frmRevenueReport(); f.MdiParent = this; f.Show(); });
            AddCard("Expense Report", Theme.AccentColor, (s, e) => { var f = new frmExpenseReport(); f.MdiParent = this; f.Show(); });
            AddCard("Availability", Theme.PrimaryDark, (s, e) => { var f = new frmVehicleAvailability(); f.MdiParent = this; f.Show(); });
            AddCard("Change Password", Theme.TextLight, (s, e) => { var f = new frmChangePassword(Program.CurrentUsername); f.ShowDialog(); });
            AddCard("About", Theme.PrimaryColor, (s, e) => { var f = new frmAbout(); f.ShowDialog(); });

            var btnLogout = new Button
            {
                Text = "Logout",
                AutoSize = true,
                Margin = new Padding(8),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Theme.DangerColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Padding = new Padding(20, 12, 20, 12)
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 50, 60);
            btnLogout.Click += (s, e) => Application.Exit();
            panel.Controls.Add(btnLogout);

            var menuPanel = new Panel { Dock = DockStyle.Top, AutoSize = true };
            menuPanel.Controls.Add(menuStrip);
            Controls.Add(menuPanel);
            Controls.Add(panel);
        }
    }
}
