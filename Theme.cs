using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    public static class Theme
    {
        public static Color PrimaryColor = Color.FromArgb(0, 100, 180);
        public static Color PrimaryLight = Color.FromArgb(0, 150, 230);
        public static Color PrimaryDark = Color.FromArgb(0, 60, 120);
        public static Color AccentColor = Color.FromArgb(255, 165, 0);
        public static Color SuccessColor = Color.FromArgb(40, 167, 69);
        public static Color DangerColor = Color.FromArgb(220, 53, 69);
        public static Color WarningColor = Color.FromArgb(255, 193, 7);
        public static Color BackgroundColor = Color.FromArgb(245, 247, 250);
        public static Color CardColor = Color.White;
        public static Color TextColor = Color.FromArgb(33, 37, 41);
        public static Color TextLight = Color.FromArgb(108, 117, 125);
        public static Color BorderColor = Color.FromArgb(220, 224, 228);
        public static Color HeaderGradientStart = Color.FromArgb(0, 80, 150);
        public static Color HeaderGradientEnd = Color.FromArgb(0, 120, 200);

        public static Font TitleFont = new Font("Segoe UI", 20F, FontStyle.Bold);
        public static Font HeadingFont = new Font("Segoe UI", 14F, FontStyle.Bold);
        public static Font BodyFont = new Font("Segoe UI", 10F);
        public static Font ButtonFont = new Font("Segoe UI", 10F, FontStyle.Bold);

        public static void ApplyButtonStyle(Button btn, Color backColor = default)
        {
            if (backColor == default) backColor = PrimaryColor;
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.Font = ButtonFont;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.AutoSize = true;
            btn.Padding = new Padding(20, 10, 20, 10);
        }

        public static void ApplyPanelStyle(Panel panel, Color backColor = default)
        {
            if (backColor == default) backColor = CardColor;
            panel.BackColor = backColor;
            panel.BorderStyle = BorderStyle.FixedSingle;
        }

        public static void ApplyLabelStyle(Label lbl, Color foreColor = default, Font font = default)
        {
            if (foreColor == default) foreColor = TextColor;
            if (font == default) font = BodyFont;
            lbl.ForeColor = foreColor;
            lbl.Font = font;
        }
    }
}
