using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameLibrary
{
    public abstract class Entity : Transform
    {
        // Properties
        public float Speed = 10f;

        // Game Properties
        public Vector2 Velocity = new Vector2();

        public Entity(Vector2 _position)
        {
            Position = _position;
            Size = new Vector2(16, 16);
        }
        public Entity(Vector2 _position, Vector2 _size)
        {
            Position = _position;
            Size = _size;
        }
        public override byte[] GetPacket()
        {
            PacketBuffer buffer = new PacketBuffer();
            // Base
            buffer.AddBytes(base.GetPacket());
            // Velocity
            buffer.AddFloat(Velocity.X);
            buffer.AddFloat(Velocity.Y);
            byte[] data = buffer.ToArray();
            buffer.Dispose();

            return data;
        }
        public override void LoadPacket(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            Position = new Vector2(buffer.GetFloat(), buffer.GetFloat());
            Scale = new Vector2(buffer.GetFloat(), buffer.GetFloat());
            Rotation = buffer.GetFloat();
            Velocity = new Vector2(buffer.GetFloat(), buffer.GetFloat());
            buffer.Dispose();
        }
    }
}
