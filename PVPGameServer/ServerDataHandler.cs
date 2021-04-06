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
            InitializeMessages();
        }

        public void InitializeMessages()
        {
            Console.WriteLine("Inisialisation des paquets réseau...");
            Packets = new Dictionary<int, Packet_>();
            Packets.Add((int)ClientPackets.ClientLogin, HandleLogin);
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
            Console.WriteLine(string.Format("Message réseau reçu de Index : {0}!", index));
        }
    }
}
