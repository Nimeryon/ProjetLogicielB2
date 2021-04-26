using System;
using System.Collections.Generic;
using System.Text;
using Bindings;

namespace PVPGameServer
{
    public class Player : Entity
    {
        public int Index;
        public double CreationTime;
        public string Pseudo;
        public Input Inputs = new Input();
        public bool IsReady = false;

        public Player(int _index, string _pseudo, float _x, float _y)
        {
            CreationTime = Game.GetTime();
            Index = _index;
            Pseudo = _pseudo;
            X = _x;
            Y = _y;
        }

        public void Update()
        {
            if (Inputs.Up) Y -= Speed;
            if (Inputs.Down) Y += Speed;
            if (Inputs.Left) X -= Speed;
            if (Inputs.Right) X += Speed;
        }
        public byte[] GetPacket()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInt(Index);
            buffer.AddFloat(X);
            buffer.AddFloat(Y);
            byte[] data = buffer.ToArray();
            buffer.Dispose();

            return data;
        }
    }

    public class Input
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;

        public Input(bool _up = false, bool _down = false, bool _left = false, bool _right = false)
        {
            Up = _up;
            Down = _down;
            Left = _left;
            Right = _right;
        }
    }
}