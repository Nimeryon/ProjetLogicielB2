using System;
using System.Collections.Generic;
using System.Text;
using GeonBit.UI;
using Microsoft.Xna.Framework;
using PVPGameLibrary;

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
            Packets.Add((int)ServerPackets.ServerConnected, HandleServerConnected);
            Packets.Add((int)ServerPackets.ServerPlayerConnect, HandlePlayerConnect);
            Packets.Add((int)ServerPackets.ServerPlayerDisconnect, HandlePlayerDisconnect);
            Packets.Add((int)ServerPackets.ServerPlayersState, HandlePlayersState);
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
            // Get index of player
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            buffer.GetInt();
            int index = buffer.GetInt();
            buffer.Dispose();

            GameHandler.CurrentPlayerIndex = index;

            GameHandler.I.Connected();
        }
        private void HandlePlayerConnect(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            buffer.GetInt();
            int nbrPlayer = buffer.GetInt();
            for (int i = 0; i < nbrPlayer; i++)
            {
                int index = buffer.GetInt();
                string pseudo = buffer.GetString();
                int character = buffer.GetInt();
                float x = buffer.GetFloat();
                float y = buffer.GetFloat();

                PlayerCharacter characterType = PlayerCharacter.Frog;
                switch (character)
                {
                    case 0:
                        characterType = PlayerCharacter.Frog;
                        break;
                    case 1:
                        characterType = PlayerCharacter.Mask;
                        break;
                    case 2:
                        characterType = PlayerCharacter.Pink;
                        break;
                    case 3:
                        characterType = PlayerCharacter.Virtual;
                        break;
                }

                bool isCurrentPlayer = index == GameHandler.CurrentPlayerIndex;
                Console.WriteLine(string.Format("{0} joueur: {1}.{2} | x:{3} / y:{4}", isCurrentPlayer ? "Votre" : "Nouveau", pseudo, index, x, y));
                if (GameHandler.Players[index] == null) GameHandler.Players[index] = new Player(index, pseudo, characterType, GameHandler._texture, new Vector2(x, y), isCurrentPlayer);
            }
            buffer.Dispose();
        }
        private void HandlePlayerDisconnect(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            buffer.GetInt();
            int index = buffer.GetInt();
            buffer.Dispose();

            if (index != GameHandler.CurrentPlayerIndex)
            {
                GameHandler.Players[index].Dispose();
                GameHandler.Players[index] = null;
                Console.WriteLine(string.Format("Déconnexion du joueur: {0}", index));
            }
        }
        private void HandlePlayersState(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            buffer.GetInt();
            int nbrPlayer = buffer.GetInt();
            for (int i = 0; i < nbrPlayer; i++)
            {
                int index = buffer.GetInt();
                //Console.WriteLine(string.Format("{0} joueur {1} ce déplace en x:{2} / y:{3}", isCurrentPlayer ? "Votre" : "Le", index, x, y));
                if (GameHandler.Players[index] != null)
                {
                    // Create packet for the player
                    PacketBuffer playerBuffer = new PacketBuffer();
                    // Position
                    playerBuffer.AddFloat(buffer.GetFloat());
                    playerBuffer.AddFloat(buffer.GetFloat());
                    // Scales
                    playerBuffer.AddFloat(buffer.GetFloat());
                    playerBuffer.AddFloat(buffer.GetFloat());
                    // Rotation
                    playerBuffer.AddFloat(buffer.GetFloat());
                    // Velocity
                    playerBuffer.AddFloat(buffer.GetFloat());
                    playerBuffer.AddFloat(buffer.GetFloat());
                    // Grounded
                    playerBuffer.AddBool(buffer.GetBool());
                    GameHandler.Players[index].LoadPacket(playerBuffer.ToArray());
                    playerBuffer.Dispose();
                }
            }
            buffer.Dispose();
        }
    }
}