using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Bindings;

namespace PVPGameClient
{
    class ClientTCP
    {
        public TcpClient Socket;
        private static NetworkStream Stream;

        private ClientDataHandler DataHandler;
        private byte[] AsyncBuff;
        private bool Connecting;
        private bool Connected;

        public ClientTCP(bool connect = true)
        {
            if (connect) ConnectToServer();
        }
       
        public void ConnectToServer()
        {
            if (Socket != null)
            {
                if (Socket.Connected || Connected) return;
                Socket.Close();
                Socket = null;
            }

            Socket = new TcpClient();
            Socket.ReceiveBufferSize = 4096;
            Socket.SendBufferSize = 4096;
            Socket.NoDelay = false;
            Array.Resize(ref AsyncBuff, Socket.ReceiveBufferSize + Socket.SendBufferSize);
            Socket.BeginConnect("127.0.0.1", 1234, new AsyncCallback(ConnectCallback), Socket);
            Connecting = true;
        }

        public void ConnectCallback(IAsyncResult asyncResult)
        {
            Socket.EndConnect(asyncResult);
            if (Socket.Connected == false)
            {
                Connecting = false;
                Connected = false;
                return;
            }

            Socket.ReceiveBufferSize = 4096;
            Socket.SendBufferSize = 4096;
            Socket.NoDelay = true;
            Stream = Socket.GetStream();
            Stream.BeginRead(AsyncBuff, 0, Socket.ReceiveBufferSize + Socket.SendBufferSize, OnReceive, null);
            Connecting = false;
            Connected = true;
        }

        public void OnReceive(IAsyncResult asyncResult)
        {
            int byteAmount = Stream.EndRead(asyncResult);
            byte[] bytes = null;
            Array.Resize(ref bytes, byteAmount);
            Buffer.BlockCopy(AsyncBuff, 0, bytes, 0, byteAmount);

            if (byteAmount == 0) return;

            DataHandler.HandleNetworkMessages(bytes);
            Stream.BeginRead(AsyncBuff, 0, Socket.ReceiveBufferSize + Socket.SendBufferSize, OnReceive, null);
        }

        public void SendData(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            Stream.Write(buffer.ToArray(), 0, buffer.Count());
            buffer.Dispose();
        }

        public void SendLogin()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInt((int)ClientPackets.ClientLogin);
            buffer.AddString("Nimerya");
            buffer.AddString("Kevin");
            SendData(buffer.ToArray());
            buffer.Dispose();
        }
    }
}
