﻿using System;
using System.Collections.Generic;
using System.Text;
using Bindings;
using GeonBit.UI;
using Microsoft.Xna.Framework;

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
            Console.WriteLine("Serveur connecté!");
            GameHandler.I.ConnexionTimer.Dispose();
            GameHandler.ClienTCP.SendLogin(GameHandler.I.ConnexionPanel.Pseudo.Value);
            GameHandler.I.ConnexionPanel.Reset();
            GameHandler.I.ConnexionPanel.Visible = false;
            GameHandler.I.AwaitConnexion = false;

            UserInterface.Active.RemoveEntity(GameHandler.I.ConnexionPanel);

            // Get index of player
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            buffer.GetInt();
            int index = buffer.GetInt();
            buffer.Dispose();

            GameHandler.CurrentPlayerIndex = index;
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
                float x = buffer.GetFloat();
                float y = buffer.GetFloat();

                bool isCurrentPlayer = index == GameHandler.CurrentPlayerIndex;
                Console.WriteLine(string.Format("{0} joueur: {1}.{2} | x:{3} / y:{4}", isCurrentPlayer ? "Votre" : "Nouveau", pseudo, index, x, y));
                if (GameHandler.Players[index] == null) GameHandler.Players[index] = new Player(index, GameHandler._texture, new Vector2(x, y), isCurrentPlayer);
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
                float x = buffer.GetFloat();
                float y = buffer.GetFloat();

                bool isCurrentPlayer = index == GameHandler.CurrentPlayerIndex;
                Console.WriteLine(string.Format("{0} joueur {1} ce déplace en x:{2} / y:{3}", isCurrentPlayer ? "Votre" : "Le", index, x, y));
                if (GameHandler.Players[index] != null && !isCurrentPlayer) GameHandler.Players[index].Move(new Vector2(x, y));
            }
            buffer.Dispose();
        }
    }
}