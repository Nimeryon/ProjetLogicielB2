using System;
using Bindings;

namespace PVPGameServer
{
    class General
    {
        private ServerTCP serverTCP;

        public void InitialiseServer()
        {
            serverTCP = new ServerTCP();
            serverTCP.InitialiseNetwork();
            Console.WriteLine("Le serveur à démarré");
        }
    }
}
