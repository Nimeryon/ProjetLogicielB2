using System;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace PVPGameClient
{
    class ClientTCP
    {
        public TcpClient Socket;
        public NetworkStream Stream;

        public ClientTCP()
        {
            ConnectToServer();
        }

        public void ConnectToServer()
        {
            Socket = new TcpClient();
            Socket.ReceiveBufferSize = 4096;
            Socket.SendBufferSize = 4096;
            Socket.NoDelay = false;
            Socket.BeginConnect("127.0.0.1", 1234, ConnectCallback, Socket);
        }

        public void ConnectCallback(IAsyncResult asyncResult)
        {

        }
    }
}
