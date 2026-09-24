using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmSplash : Form
    {
        private Label lblTitle;
        private Label lblSubtitle;
        private Timer timer1;
        private ProgressBar progressBar;
        private Label lblLoading;

        public frmSplash()
        {
            System.IO.File.AppendAllText("applog.txt", "frmSplash constructor called\r\n");
            InitializeComponent();
            Load += frmSplash_Load;
        }

        private void InitializeComponent()
        {
            lblTitle = new Label
            {
                Text = "VEHICLE RENTAL & BILLING",
                Font = Theme.TitleFont,
                ForeColor = Theme.PrimaryColor,
                AutoSize = true,
                Location = new Point((Screen.PrimaryScreen.Bounds.Width - 400) / 2, 120)
            };

            lblSubtitle = new Label
            {
                Text = "College Project",
                Font = Theme.HeadingFont,
                ForeColor = Theme.TextLight,
                AutoSize = true,
                Location = new Point((Screen.PrimaryScreen.Bounds.Width - 200) / 2, 170)
            };

            progressBar = new ProgressBar
            {
                Width = 300,
                Height = 8,
                Location = new Point((Screen.PrimaryScreen.Bounds.Width - 300) / 2, 220),
                Style = ProgressBarStyle.Continuous,
                Maximum = 100,
                Value = 0
            };

            lblLoading = new Label
            {
                Text = "Loading...",
                Font = Theme.BodyFont,
                ForeColor = Theme.TextLight,
                AutoSize = true,
                Location = new Point((Screen.PrimaryScreen.Bounds.Width - 100) / 2, 235)
            };

            timer1 = new Timer { Interval = 30 };
            timer1.Tick += Timer1_Tick;

            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(progressBar);
            Controls.Add(lblLoading);

            Text = "Vehicle Rental & Billing System";
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Theme.BackgroundColor;
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            System.IO.File.AppendAllText("applog.txt", "Splash load\r\n");
            timer1.Start();
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
            System.IO.File.AppendAllText("applog.txt", "ShowLogin called\r\n");
            Close();
            System.IO.File.AppendAllText("applog.txt", "Splash closed\r\n");
            using (var login = new frmLogin())
            {
                var result = login.ShowDialog();
                System.IO.File.AppendAllText("applog.txt", "DialogResult: " + result + "\r\n");
                if (result == DialogResult.OK)
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
