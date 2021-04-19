using System;

namespace PVPGameServer
{
    class General
    {
        public static ServerTCP serverTCP;
        public static ServerDataHandler DataHandler;

        public void InitialiseServer()
        {
            serverTCP = new ServerTCP();
            serverTCP.InitialiseNetwork();
            DataHandler = new ServerDataHandler();
            Console.WriteLine("Le serveur à démarré");
        }
    }
}
