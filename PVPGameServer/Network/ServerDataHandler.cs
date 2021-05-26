using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using PVPGameLibrary;

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
            string character = buffer.GetString();
            Console.WriteLine(string.Format("Message de Index {0} : {1}!", index, pseudo));
            buffer.Dispose();

            Game.AddPlayer(index, pseudo, character);
        }
        private void HandleInputs(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            buffer.GetInt();
            bool left = buffer.GetBool();
            bool right = buffer.GetBool();
            bool jump = buffer.GetBool();
            bool attack = buffer.GetBool();
            Console.WriteLine(string.Format("Inputs de Index {0} : Left:{1} / Right:{2} / Jump:{3} / Attack:{4}", index, left, right, jump, attack));
            buffer.Dispose();

            Game.Players[index].Inputs = new Inputs(left, right, jump, attack);
        }
    }
}