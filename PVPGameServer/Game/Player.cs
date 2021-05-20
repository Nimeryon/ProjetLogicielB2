using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Bindings;
using PVPGameLibrary;

namespace PVPGameServer
{
    public class Player : PVPGameLibrary.Player
    {
        public double CreationTime;
        public bool IsReady = false;

        public Player(int _index, string _pseudo, Vector2 _position) : base(_index, _pseudo, _position)
        {
            CreationTime = Game.GetTime();
        }

        public override float GetDeltaTime()
        {
            return Game.Deltatime;
        }
        public override void GetInputs()
        {
            return;
        }
        public byte[] GetPacket()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInt(Index);
            buffer.AddFloat(Position.X);
            buffer.AddFloat(Position.Y);
            byte[] data = buffer.ToArray();
            buffer.Dispose();

            return data;
        }
    }
}