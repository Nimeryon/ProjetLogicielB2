using System;
using System.Threading;

namespace PVPGameServer
{
    class Program
    {
        private static Thread consoleThread;
        private static General general;

        static void Main(string[] args)
        {
            general = new General();
            consoleThread = new Thread(new ThreadStart(ConsoleThread));
            consoleThread.Start();
            general.InitialiseServer();

            // Start game
            Timer gameTimer = new Timer(GameThread, false, 0, 1000 / Game.ServerFrame);
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