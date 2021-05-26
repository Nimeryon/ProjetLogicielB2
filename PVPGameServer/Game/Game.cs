using Microsoft.Xna.Framework;
using PVPGameLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameServer
{
    class Game
    {
        public static int ServerFrame = 20;
        public static DateTime StartTime = DateTime.Now;
        public static Player[] Players = new Player[Constants.MAX_PLAYERS];
        public static Point Size = new Point();
        public static Grid Grid = new Grid();

        // Spawn Positions for player
        public static Vector2[] SpawnPositions = new Vector2[]
        {
            new Vector2(),
            new Vector2()
        };

        // Deltatime calculations
        public static float Deltatime;
        static DateTime lastTime = DateTime.Now;

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
        public static void AddPlayer(int _index, string _pseudo, string _character)
        {
            PlayerCharacter character = PlayerCharacter.Frog;
            switch (_character)
            {
                case "frog":
                    character = PlayerCharacter.Frog;
                    break;
                case "mask":
                    character = PlayerCharacter.Mask;
                    break;
                case "pink":
                    character = PlayerCharacter.Pink;
                    break;
                case "virtual":
                    character = PlayerCharacter.Virtual;
                    break;
            }

            Players[_index] = new Player(_index, _pseudo, character, new Vector2(Helpers.RandomRange(20f, 1080f), 500f));
            ServerTCP.Clients[_index].SendPlayerConnect();
            Players[_index].IsReady = true;
        }
        public static void Update()
        {
            // Deltatime calculations
            DateTime currentTime = DateTime.Now;
            Deltatime = (float)currentTime.Subtract(lastTime).TotalSeconds * 10;
            lastTime = currentTime;

            // Packets creation
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