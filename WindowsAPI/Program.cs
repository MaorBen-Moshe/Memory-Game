using System.Windows.Forms;

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
