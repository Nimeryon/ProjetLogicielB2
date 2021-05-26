using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameLibrary
{
    public class Transform
    {
        // Position
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                OldPosition = _position;
                _position = value;
            }
        }
        public Vector2 OldPosition;
        private Vector2 _position = new Vector2();

        // Scale
        public Vector2 Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                OldScale = _scale;
                _scale = value;
            }
        }
        public Vector2 OldScale;
        private Vector2 _scale = Vector2.One;

        // Rotate
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                OldRotation = _rotation;
                _rotation = value;
            }
        }
        public float OldRotation;
        private float _rotation = 0;

        // Size
        public Vector2 Size
        {
            get
            {
                return _size;
            }
            set
            {
                OldSize = _size;
                _size = value;
            }
        }
        public Vector2 OldSize;
        private Vector2 _size = new Vector2();

        // Bounds
        public Rectangle Bounds
        {
            get
            {
                return GetBounds();
            }
            set
            {
                _bounds = value;
            }
        }
        private Rectangle _bounds = new Rectangle();

        // Functions
        public virtual Rectangle GetBounds()
        {
            _bounds = new Rectangle(Position.ToPoint(), Size.ToPoint() * Scale.ToPoint());
            return _bounds;
        }
        public virtual void Move(Vector2 _force)
        {
            Position += _force;
        }
        public virtual void MoveAt(Vector2 _position)
        {
            Position = _position;
        }
        public virtual void Rotate(float _force)
        {
            Rotation += _force;
        }
        public static void Lerp(ref Transform transform1, ref Transform transform2, float amount, ref Transform result)
        {
            result.Position = Vector2.Lerp(transform1.Position, transform1.Position, amount);
            result.Scale = Vector2.Lerp(transform1.Scale, transform2.Scale, amount);
            result.Rotation = MathHelper.Lerp(transform1.Rotation, transform2.Rotation, amount);
        }
        public virtual byte[] GetPacket()
        {
            PacketBuffer buffer = new PacketBuffer();
            // Position
            buffer.AddFloat(Position.X);
            buffer.AddFloat(Position.Y);
            // Scale
            buffer.AddFloat(Scale.X);
            buffer.AddFloat(Scale.Y);
            // Rotation
            buffer.AddFloat(Rotation);
            byte[] data = buffer.ToArray();
            buffer.Dispose();

            return data;
        }
        public virtual void LoadPacket(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            Position = new Vector2(buffer.GetFloat(), buffer.GetFloat());
            Scale = new Vector2(buffer.GetFloat(), buffer.GetFloat());
            Rotation = buffer.GetFloat();
            buffer.Dispose();
        }
    }
}