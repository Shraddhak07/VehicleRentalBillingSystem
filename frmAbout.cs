using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmAbout : Form
    {
        public frmAbout() => InitializeComponent();

        private void InitializeComponent()
        {
            var header = new Panel
            {
                Size = new Size(500, 120),
                BackColor = Theme.PrimaryColor
            };

            var lblIcon = new Label
            {
                Text = "🚗",
                Font = new Font("Segoe UI", 40F),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(220, 20)
            };

            var lblTitle = new Label
            {
                Text = "VEHICLE RENTAL",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(140, 70)
            };

            header.Controls.Add(lblIcon);
            header.Controls.Add(lblTitle);

            var lblDesc = new Label
            {
                Text = "Vehicle Rental & Billing System\n\n" +
                       "College Project\n" +
                       "Version: 1.0\n" +
                       "Platform: Windows Forms (.NET)\n" +
                       "Database: Microsoft Access\n\n" +
                       "Author: Student Developer Team",
                Font = Theme.BodyFont,
                ForeColor = Theme.TextColor,
                AutoSize = true,
                Location = new Point(30, 140),
                MaximumSize = new Size(450, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var btnClose = new Button { Text = "Close", Width = 120, Location = new Point(190, 380) };
            btnClose.FlatStyle = FlatStyle.Flat;
            Theme.ApplyButtonStyle(btnClose, Theme.PrimaryColor);
            btnClose.FlatAppearance.MouseOverBackColor = Theme.PrimaryLight;
            btnClose.Click += (s, e) => Close();

            Controls.Add(header);
            Controls.Add(lblDesc);
            Controls.Add(btnClose);
            Text = "About";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(500, 450);
            BackColor = Theme.BackgroundColor;
        }
    }
}
