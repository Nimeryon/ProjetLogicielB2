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
            Packets.Add((int)ClientPackets.ClientInputs, HandleInputs);
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
            string pseudo = buffer.GetString();
            Console.WriteLine(string.Format("Message de Index {0} : {1}!", index, pseudo));
            buffer.Dispose();

            Game.Players[index] = new Player(index, pseudo, Helpers.RandomRange(20f, 300f), Helpers.RandomRange(20f, 300f));
            ServerTCP.Clients[index].SendPlayerConnect();
            Game.Players[index].IsReady = true;
        }
        private void HandleInputs(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            buffer.GetInt();
            bool up = buffer.GetBool();
            bool down = buffer.GetBool();
            bool left = buffer.GetBool();
            bool right = buffer.GetBool();
            Console.WriteLine(string.Format("Inputs de Index {0} : Up:{1} / Down:{2} / Left:{3} / Right:{4}", index, up, down, left, right));
            buffer.Dispose();

            Game.Players[index].Inputs = new Input(up, down, left, right);
        }
    }
}