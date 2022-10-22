namespace SlimeLab
{
    internal static class Program
    {
        private static void Main()
        {
            using Startup game = new();
            game.Run();
        }
    }
}