using System.Windows.Forms;

namespace SlimeLab
{
    internal static class Program
    {
        private static void Main()
        {
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            using Core game = new();
            game.Run();
        }
    }
}