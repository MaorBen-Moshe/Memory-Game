using System.Windows.Forms;
using System;
using System.Drawing;

namespace WindowsAPI
{
    public class Program
    {
        public static void Main()
        {
            Application.EnableVisualStyles();
            new SettingsForm().ShowDialog();
        }
    }
}
