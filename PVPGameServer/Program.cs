using System;
using System.Threading;

namespace PVPGameServer
{
    class Program
    {
        private static Thread consoleThread;
        private static General general;

        private static int ServerFrame = 20;

        static void Main(string[] args)
        {
            general = new General();
            consoleThread = new Thread(new ThreadStart(ConsoleThread));
            consoleThread.Start();
            general.InitialiseServer();

            // Start game
            Timer gameTimer = new Timer(GameThread, false, 0, 1000 / ServerFrame);
        }

        static void ConsoleThread()
        {
            Console.ReadLine();
        }
        static void GameThread(object state)
        {
            Game.Update();
        }
    }
}