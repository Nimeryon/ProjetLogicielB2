using System;
using System.Net.Sockets;
using Bindings;

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
                    CloseSocket();
                    return;
                }

                byte[] newBytes = null;
                Array.Resize(ref newBytes, readBytes);
                Buffer.BlockCopy(ReadBuff, 0, newBytes, 0, readBytes);

                // Data
                General.DataHandler.HandleNetworkMessages(Index, newBytes);
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
            SendPlayerDisconnect();
        }
        public void SendData(byte[] data)
        {
            if (Stream == null) Stream = Socket.GetStream();

            try
            {
                PacketBuffer buffer = new PacketBuffer();
                buffer.AddBytes(data);
                Stream.Write(buffer.ToArray(), 0, buffer.Count());
                buffer.Dispose();
            }
            catch (Exception e)
            {
                CloseSocket();
            }
        }

        // Sender
        public void SendServerConnected()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInt((int)ServerPackets.ServerConnected);
            buffer.AddInt(Index);
            SendData(buffer.ToArray());
            buffer.Dispose();
            Console.WriteLine(string.Format("Envoie du message de connexion au client {0}.", Index));
        }
        public void SendPlayerConnect()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInt((int)ServerPackets.ServerPlayerConnect);
            buffer.AddInt(Game.GetPlayersNumber());
            // Create buffer with every player
            for (int i = 0; i < Game.Players.Length; i++)
            {
                Player player = Game.Players[i];
                if (player == null) break;

                buffer.AddInt(i);
                buffer.AddString(player.Pseudo);
                buffer.AddFloat(player.Position.X);
                buffer.AddFloat(player.Position.Y);
            }
            ServerTCP.SendData(buffer.ToArray());
            buffer.Dispose();
            Console.WriteLine("Envoie à tout les clients la liste des joueurs.");
        }
        public void SendPlayerDisconnect()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInt((int)ServerPackets.ServerPlayerDisconnect);
            buffer.AddInt(Index);
            ServerTCP.SendData(buffer.ToArray());
            buffer.Dispose();
            Console.WriteLine(string.Format("Envoie à tout les clients la déconnexion du joueur {0}.", Index));

            // Delete player
            Game.Players[Index] = null;
        }
        public void SendPlayersState(byte[] data)
        {
            if (Game.Players[Index] != null && Game.Players[Index].IsReady == false) return;

            ServerTCP.SendData(data);
        }
    }
}