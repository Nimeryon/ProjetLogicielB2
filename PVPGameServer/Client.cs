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

        public Client(int index, string ip, TcpClient socket)
        {
            Index = index;
            Ip = ip;
            Socket = socket;
            Start();
        }

        public void Start()
        {
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
                    CloseSocket(Index);
                    return;
                }

                byte[] newBytes = null;
                Array.Resize(ref newBytes, readBytes);
                Buffer.BlockCopy(ReadBuff, 0, newBytes, 0, readBytes);
                // Data
                Stream.BeginRead(ReadBuff, 0, Socket.ReceiveBufferSize, OnReceiveData, null);
            }
            catch
            {
                CloseSocket(Index);
            }
        }

        public void CloseSocket(int index)
        {
            Console.WriteLine(string.Format("La connexion avec {0} à étais coupé.", Ip));
            Socket.Close();
            Socket = null;
        }
    }
}
