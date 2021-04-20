using System;
using System.Collections.Generic;
using System.Text;

namespace Bindings.Game
{
    class PlayerState
    {
        public bool Up = false;
        public bool Down = false;
        public bool Left = false;
        public bool Right = false;

        public byte[] GetState()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBool(Up);
            buffer.AddBool(Down);
            buffer.AddBool(Left);
            buffer.AddBool(Right);

            byte[] data = buffer.ToArray();
            buffer.Dispose();

            return data;
        }
    }
}