using System;
using System.Windows.Forms;

namespace VehicleRentalApp
{
    static class Program
    {
        public static string CurrentUsername { get; set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
            Application.Run(new frmSplash());
        }
    }
}
