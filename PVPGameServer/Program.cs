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
        }

        static void ConsoleThread()
        {
            Console.ReadLine();
        }
    }
}
