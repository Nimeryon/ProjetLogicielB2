using System;

namespace PVPGameServer
{
    class General
    {
        private ServerTCP serverTCP;
        private ServerDataHandler DataHandler;

        public void InitialiseServer()
        {
            serverTCP = new ServerTCP();
            serverTCP.InitialiseNetwork();
            DataHandler = new ServerDataHandler();
            Console.WriteLine("Le serveur à démarré");
        }
    }
}
