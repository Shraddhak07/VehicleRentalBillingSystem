using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmSplash : Form
    {
        private Label lblTitle;
        private Timer timer1;

        public frmSplash() => InitializeComponent();

        private void InitializeComponent()
        {
            lblTitle = new Label
            {
                Text = "Vehicle Rental & Billing System",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

            timer1 = new Timer { Interval = 2000 };
            timer1.Tick += Timer1_Tick;

            Controls.Add(lblTitle);
            Text = "Loading...";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
        }

        private void frmSplash_Load(object sender, EventArgs e) => timer1.Start();

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Close();
            ShowLogin();
        }

        private void ShowLogin()
        {
            using (var login = new frmLogin())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new frmMainMenu());
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}
