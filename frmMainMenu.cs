using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmMainMenu : Form
    {
        public frmMainMenu()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            IsMdiContainer = true;
            Text = "Vehicle Rental & Billing System - Main Menu";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.Manual;

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(10, 10, 10, 10)
            };

            void AddButton(string text, EventHandler handler)
            {
                var btn = new Button { Text = text, AutoSize = true, Margin = new Padding(5), Font = new Font("Segoe UI", 10F) };
                btn.Click += handler;
                panel.Controls.Add(btn);
            }

            AddButton("Add Vehicle", (s, e) => { var f = new frmAddVehicle(); f.MdiParent = this; f.Show(); });
            AddButton("Manage Vehicles", (s, e) => { var f = new frmManageVehicles(); f.MdiParent = this; f.Show(); });
            AddButton("Add Customer", (s, e) => { var f = new frmAddCustomer(); f.MdiParent = this; f.Show(); });
            AddButton("Manage Customers", (s, e) => { var f = new frmManageCustomers(); f.MdiParent = this; f.Show(); });
            AddButton("Book Rental", (s, e) => { var f = new frmBookRental(); f.MdiParent = this; f.Show(); });
            AddButton("Return Vehicle", (s, e) => { var f = new frmReturnVehicle(); f.MdiParent = this; f.Show(); });
            AddButton("Maintenance Expenses", (s, e) => { var f = new frmMaintenanceExpenses(); f.MdiParent = this; f.Show(); });
            AddButton("Rental History", (s, e) => { var f = new frmRentalHistory(); f.MdiParent = this; f.Show(); });
            AddButton("Revenue Report", (s, e) => { var f = new frmRevenueReport(); f.MdiParent = this; f.Show(); });
            AddButton("Expense Report", (s, e) => { var f = new frmExpenseReport(); f.MdiParent = this; f.Show(); });
            AddButton("Vehicle Availability", (s, e) => { var f = new frmVehicleAvailability(); f.MdiParent = this; f.Show(); });
            AddButton("Change Password", (s, e) => { var f = new frmChangePassword { Username = Program.CurrentUsername }; f.ShowDialog(); });
            AddButton("About", (s, e) => { var f = new frmAbout(); f.ShowDialog(); });

            var btnExit = new Button { Text = "Exit", AutoSize = true, Margin = new Padding(5), Font = new Font("Segoe UI", 10F) };
            btnExit.Click += (s, e) => Application.Exit();
            panel.Controls.Add(btnExit);

            Controls.Add(panel);
        }
    }
}
