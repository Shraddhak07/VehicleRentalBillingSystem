using System;
using System.IO;
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

            Application.ThreadException += (s, e) => LogException(e.Exception);

            try
            {
                using (var splash = new frmSplash())
                {
                    Application.Run(splash);
                }

                using (var login = new frmLogin())
                {
                    if (login.ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new frmMainMenu());
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                MessageBox.Show("Application crashed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void LogException(Exception ex)
        {
            try
            {
                File.AppendAllText("applog.txt", DateTime.Now + ": " + ex + Environment.NewLine);
            }
            catch { }
        }
    }
}
