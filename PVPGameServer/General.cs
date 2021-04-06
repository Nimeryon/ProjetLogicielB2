using System;

namespace PVPGameServer
{
    class General
    {
        private ServerTCP serverTCP;
        private ServerDataHandler DataHandler;

        public void InitialiseServer()
        {
            DataHandler = new ServerDataHandler();
            serverTCP = new ServerTCP();
            serverTCP.InitialiseNetwork();
            Console.WriteLine("Le serveur à démarré");
        }
    }
}
