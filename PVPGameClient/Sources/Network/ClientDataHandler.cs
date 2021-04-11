using System;
using System.Collections.Generic;
using System.Text;
using Bindings;

namespace PVPGameClient
{
    class ClientDataHandler
    {
        public PacketBuffer Buffer = new PacketBuffer();

        private delegate void Packet_(byte[] data);
        private Dictionary<int, Packet_> Packets;

        public ClientDataHandler()
        {
            InitializeMessages();
        }

        public void InitializeMessages()
        {
            Packets = new Dictionary<int, Packet_>();
        }

        public void HandleNetworkMessages(byte[] data)
        {
            int packetNum;
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            packetNum = buffer.GetInt();
            buffer.Dispose();

            if (Packets.TryGetValue(packetNum, out Packet_ Packet)) Packet.Invoke(data);
        }
    }
}
