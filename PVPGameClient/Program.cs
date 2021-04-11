using System;

namespace PVPGameClient
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameHandler())
                game.Run();
        }
    }
}
