using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public partial class frmAbout : Form
    {
        public frmAbout() => InitializeComponent();

        private void InitializeComponent()
        {
            var lbl = new Label
            {
                Text = "Vehicle Rental & Billing System\n\n" +
                       "College Project\n" +
                       "Version: 1.0\n" +
                       "Platform: Windows Forms (.NET)\n" +
                       "Database: Microsoft Access\n\n" +
                       "Author: Student Developer Team",
                AutoSize = true,
                Font = new Font("Segoe UI", 12F),
                MaximumSize = new Size(500, 0)
            };
            Controls.Add(lbl);
            Text = "About";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            var okButton = new Button { Text = "OK", DialogResult = DialogResult.OK };
            AcceptButton = okButton;
            Controls.Add(okButton);
        }
    }
}
