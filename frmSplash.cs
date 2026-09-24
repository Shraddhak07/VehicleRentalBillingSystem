using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmSplash : Form
    {
        private Label lblTitle;
        private Label lblSubtitle;
        private Timer timer1;
        private ProgressBar progressBar;
        private Panel headerPanel;

        public frmSplash() => InitializeComponent();

        private void InitializeComponent()
        {
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 180,
                BackColor = Theme.PrimaryColor
            };

            lblTitle = new Label
            {
                Text = "VEHICLE RENTAL",
                Font = new Font("Segoe UI", 28F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 50,
                Padding = new Padding(0, 25, 0, 0)
            };

            lblSubtitle = new Label
            {
                Text = "& BILLING SYSTEM",
                Font = new Font("Segoe UI", 16F, FontStyle.Regular),
                ForeColor = Color.FromArgb(200, 220, 255),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 40
            };

            headerPanel.Controls.Add(lblSubtitle);
            headerPanel.Controls.Add(lblTitle);

            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.BackgroundColor
            };

            var lblTitle2 = new Label
            {
                Text = "VEHICLE RENTAL & BILLING",
                Font = Theme.TitleFont,
                ForeColor = Theme.PrimaryColor,
                AutoSize = true,
                Location = new Point(200, 60)
            };

            lblSubtitle = new Label
            {
                Text = "College Project",
                Font = Theme.HeadingFont,
                ForeColor = Theme.TextLight,
                AutoSize = true,
                Location = new Point(240, 100)
            };

            progressBar = new ProgressBar
            {
                Width = 300,
                Height = 8,
                Location = new Point(200, 160),
                Style = ProgressBarStyle.Continuous,
                Maximum = 100,
                Value = 0
            };

            var loadingLabel = new Label
            {
                Text = "Loading...",
                Font = Theme.BodyFont,
                ForeColor = Theme.TextLight,
                AutoSize = true,
                Location = new Point(220, 175)
            };

            contentPanel.Controls.Add(loadingLabel);
            contentPanel.Controls.Add(progressBar);
            contentPanel.Controls.Add(lblSubtitle);
            contentPanel.Controls.Add(lblTitle);
            Controls.Add(contentPanel);

            timer1 = new Timer { Interval = 30 };
            timer1.Tick += Timer1_Tick;

            Text = "Vehicle Rental & Billing System";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Theme.BackgroundColor;
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblTitle.Parent = this;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar.Value < 100)
            {
                progressBar.Value += 5;
            }
            else
            {
                timer1.Stop();
                ShowLogin();
            }
        }

        private void ShowLogin()
        {
            Close();
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
