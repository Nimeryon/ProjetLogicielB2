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

        public ServerDataHandler()
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

        // Handler
        private void HandleLogin(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            buffer.GetInt();
            string text = buffer.GetString();
            Console.WriteLine(string.Format("Message de Index {0} : {1}!", index, text));
            buffer.Dispose();

            ServerTCP.Clients[index].SendOK();
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