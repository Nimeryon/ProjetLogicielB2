using System;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace PVPGameServer
{
    class Client
    {
        public int Index;
        public string Ip;
        public TcpClient Socket;
        public NetworkStream Stream;
        public bool Closing;
        public byte[] ReadBuff;

        private ServerDataHandler DataHandler;

        public Client(int index, string ip, TcpClient socket)
        {
            Index = index;
            Ip = ip;
            Socket = socket;
            Start();
        }

        public void Start()
        {
            DataHandler = new ServerDataHandler();
            Socket.SendBufferSize = 4096;
            Socket.ReceiveBufferSize = 4096;
            Stream = Socket.GetStream();
            // Set the size of the ReadBuff Array
            Array.Resize(ref ReadBuff, Socket.ReceiveBufferSize);
            Stream.BeginRead(ReadBuff, 0, Socket.ReceiveBufferSize, OnReceiveData, null);
        }

        public void OnReceiveData(IAsyncResult asyncResult)
        {
            try
            {
                int readBytes = Stream.EndRead(asyncResult);
                if (readBytes <= 0)
                {
                    CloseSocket();
                    return;
                }

                byte[] newBytes = null;
                Array.Resize(ref newBytes, readBytes);
                Buffer.BlockCopy(ReadBuff, 0, newBytes, 0, readBytes);
                // Data
                DataHandler.HandleNetworkMessages(Index, newBytes);
                Stream.BeginRead(ReadBuff, 0, Socket.ReceiveBufferSize, OnReceiveData, null);
            }
            catch
            {
                CloseSocket();
            }
        }

        public void CloseSocket()
        {
            Console.WriteLine(string.Format("Index {0}:{1} à étais coupé.", Index, Ip));
            Socket.Close();
            ServerTCP.Clients[Index] = null;
        }
    }
}
