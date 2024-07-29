using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DangKyKhamTuDong
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.CurrentCulture = new System.Globalization.CultureInfo("vi-VN", false);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frm_DangKyKham());
        }
    }
}
