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

        public Entity()
        {
            return;
        }
        public Entity(Vector2 _position)
        {
            Position = _position;
        }
        public Entity(Vector2 _position, Vector2 _size)
        {
            Position = _position;
            Size = _size;
        }
    }
}
