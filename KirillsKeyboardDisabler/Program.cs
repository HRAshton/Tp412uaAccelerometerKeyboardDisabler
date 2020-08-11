using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Tp412uaAccelerometerKeyboardDisabler
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (ProcessIcon pi = new ProcessIcon(BlockInput))
            {
                Application.Run();
                Application.ApplicationExit += (o, e) => BlockInput(false);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool BlockInput(bool fBlockIt);
    }
}
