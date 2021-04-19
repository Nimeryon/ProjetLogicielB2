using System;
using System.Collections.Generic;
using System.Text;
using Bindings;

namespace PVPGameClient
{
    public class ClientDataHandler
    {
        private delegate void Packet_(byte[] data);
        private Dictionary<int, Packet_> Packets;

        public ClientDataHandler()
        {
            Console.WriteLine("Inisialisation des paquets réseau...");
            Packets = new Dictionary<int, Packet_>();

            // Add Listener to packets
            Packets.Add((int)ServerPackets.ServerOK, HandleOK);
            Packets.Add((int)ServerPackets.ServerConnected, HandleServerConnected);
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

        // Handler
        private void HandleServerConnected(byte[] data)
        {
            Console.WriteLine("Serveur connecté!");
            GameHandler.I.ConnexionTimer.Dispose();
            GameHandler.ClienTCP.SendLogin(GameHandler.I.ConnexionPanel.Pseudo.Value);
            GameHandler.I.ConnexionPanel.Reset();
            GameHandler.I.ConnexionPanel.Visible = false;
            GameHandler.I.AwaitConnexion = false;
        }
        private void HandleOK(byte[] data)
        {
            Console.WriteLine("OK depuis le serveur!");
        }
    }
}
