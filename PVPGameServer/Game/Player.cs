using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using PVPGameLibrary;

namespace PVPGameServer
{
    public class Player : PVPGameLibrary.Player
    {
        public double CreationTime;
        public bool IsReady = false;

        public Player(int _index, string _pseudo, PlayerCharacter _character, Vector2 _position) : base(_index, _pseudo, _character, _position)
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
        public override byte[] GetPacket()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInt(Index);
            buffer.AddBytes(base.GetPacket());
            byte[] data = buffer.ToArray();
            buffer.Dispose();
            return data;
        }
    }
}