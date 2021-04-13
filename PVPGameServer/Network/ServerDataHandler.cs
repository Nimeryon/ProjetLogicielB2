using System;
using System.Collections.Generic;
using System.Text;
using Bindings;

namespace PVPGameServer
{
    class ServerDataHandler
    {
        private delegate void Packet_(int index, byte[] data);
        private Dictionary<int, Packet_> Packets;

        public ServerDataHandler(bool initialize = true)
        {
            if (initialize) InitializeMessages();
        }

        public void InitializeMessages()
        {
            Console.WriteLine("Inisialisation des paquets réseau...");
            Packets = new Dictionary<int, Packet_>();
            // Add Listener to packets
            Packets.Add((int)ClientPackets.ClientLogin, HandleLogin);
            Packets.Add((int)ClientPackets.ClientMovement, HandleMovement);
        }
        public void HandleNetworkMessages(int index, byte[] data)
        {
            int packetNum;
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            packetNum = buffer.GetInt();
            buffer.Dispose();

            if (Packets.TryGetValue(packetNum, out Packet_ Packet)) Packet.Invoke(index, data);
        }
        private void HandleLogin(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            buffer.GetInt();
            string text = buffer.GetString();
            string text2 = buffer.GetString();
            Console.WriteLine(string.Format("Message de Index {0} : {1}/{2}!", index, text, text2));
            buffer.Dispose();
        }
        private void HandleMovement(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            buffer.GetInt();
            float x = buffer.GetFloat();
            float y = buffer.GetFloat();
            Console.WriteLine(string.Format("Mouvement de Index {0} : x:{1} y:{2}", index, x, y));
            buffer.Dispose();
        }
    }
}
