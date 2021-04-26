using System;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;
using Bindings;
using System.Threading.Tasks;

namespace PVPGameServer
{
    class ServerTCP
    {
        public static Client[] Clients = new Client[Constants.MAX_PLAYERS];

        public TcpListener Socket;

        public void InitialiseNetwork()
        {
            Console.WriteLine("Initialisation du serveur...");
            Socket = new TcpListener(IPAddress.Any, 1234);
            Socket.Start();
            Socket.BeginAcceptTcpClient(OnClientConnect, null);
        }
        public void OnClientConnect(IAsyncResult asyncResult)
        {
            TcpClient client = Socket.EndAcceptTcpClient(asyncResult);
            client.NoDelay = false;
            Socket.BeginAcceptTcpClient(OnClientConnect, null);

            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (Clients[i] == null)
                {
                    Clients[i] = new Client(i, client.Client.RemoteEndPoint.ToString(), client);
                    Console.WriteLine(string.Format("Nouvelle connexion depuis {0}, Index : {1}", Clients[i].Ip, i));
                    Task.Delay(TimeSpan.FromMilliseconds(250)).ContinueWith(task => {
                        Clients[i].SendServerConnected();
                    });
                    return;
                }
            }
        }
        public static void SendData(int index, byte[] data)
        {
            if (Clients[index] != null) Clients[index].SendData(data);
        }
        public static void SendData(byte[] data)
        {
            for (int i = 0; i < Clients.Length; i++)
            {
                if (Clients[i] != null) SendData(i, data);
            }
        }
    }
}