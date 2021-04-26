using Bindings;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameServer
{
    class Game
    {
        public static DateTime StartTime = DateTime.Now;
        public static Player[] Players = new Player[Constants.MAX_PLAYERS];

        public static int GetPlayersNumber()
        {
            int nbr = 0;
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] != null) nbr++;
            }

            return nbr;
        }
        public static double GetTime()
        {
            return DateTime.Now.Subtract(StartTime).TotalMilliseconds;
        }
        public static void Update()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInt((int)ServerPackets.ServerPlayersState);
            buffer.AddInt(GetPlayersNumber());
            // Create packet
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] != null)
                {
                    Players[i].Update();
                    buffer.AddBytes(Players[i].GetPacket());
                }
            }
            // Send data to players
            byte[] data = buffer.ToArray();
            for (int i = 0; i < Players.Length; i++)
            {
                if (ServerTCP.Clients[i] != null) ServerTCP.Clients[i].SendPlayersState(data);
            }
            buffer.Dispose();
        }
    }
}