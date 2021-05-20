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
        protected Vector2 _position = new Vector2();

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
        protected Vector2 _scale = Vector2.One;

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
        protected float _rotation = 0;

        // Size
        public Vector2 Size
        {
            get
            {
                return _size * Scale;
            }
            set
            {
                OldSize = _size;
                _size = value;
            }
        }
        public Vector2 OldSize;
        protected Vector2 _size = new Vector2();

        // Bounds
        public Rectangle Bounds
        {
            get
            {
                _bounds = new Rectangle(Position.ToPoint(), Size.ToPoint());
                return _bounds; 
            }
            set
            {
                _bounds = value;
            }
        }
        protected Rectangle _bounds = new Rectangle();

        // Functions
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
    }
}