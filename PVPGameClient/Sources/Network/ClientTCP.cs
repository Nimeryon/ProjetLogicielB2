using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Microsoft.Xna.Framework;
using PVPGameLibrary;

namespace PVPGameClient
{
    public class ClientTCP
    {
        public TcpClient Socket;
        public NetworkStream Stream;

        private byte[] AsyncBuff;
        public bool Connecting;
        public bool Connected;

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
        public void Disconnect()
        {
            if (Socket == null) return;

            Connecting = false;
            Connected = false;
            Socket.Close();
            Socket = null;
        }
        public void ConnectCallback(IAsyncResult asyncResult)
        {
            if (Socket.Connected == false)
            {
                Connecting = false;
                Connected = false;
                return;
            }

            Socket.EndConnect(asyncResult);
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
            if (!Connected) return;

            try
            {
                int byteAmount = Stream.EndRead(asyncResult);
                byte[] bytes = null;
                Array.Resize(ref bytes, byteAmount);
                Buffer.BlockCopy(AsyncBuff, 0, bytes, 0, byteAmount);

                if (byteAmount == 0) return;

                GameHandler.DataHandler.HandleNetworkMessages(bytes);
                Stream.BeginRead(AsyncBuff, 0, Socket.ReceiveBufferSize + Socket.SendBufferSize, OnReceive, null);
            }
            catch
            {
                Disconnect();
                GameHandler.I.Disconnected();
            }
        }
        public void SendData(byte[] data)
        {
            if (!Connected) return;
            if (Stream == null) Stream = Socket.GetStream();

            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            Stream.Write(buffer.ToArray(), 0, buffer.Count());
            buffer.Dispose();
        }

        // Sender
        public void SendLogin(string _pseudo, string _character)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInt((int)ClientPackets.ClientLogin);
            buffer.AddString(_pseudo);
            buffer.AddString(_character);
            SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public void SendState(Inputs _state)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInt((int)ClientPackets.ClientInputs);
            buffer.AddBool(_state.Left);
            buffer.AddBool(_state.Right);
            buffer.AddBool(_state.Jump);
            buffer.AddBool(_state.Attack);
            SendData(buffer.ToArray());
            buffer.Dispose();
        }
    }
}