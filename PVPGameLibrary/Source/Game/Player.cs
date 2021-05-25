using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameLibrary
{
    public class Player : Entity, IDisposable
    {
        // Properties
        public int Index;
        public string Pseudo;

        public bool IsGrounded;
        public bool IsJumping;
        public Point GridPos;
        public float PreviousBottom;

        // Constants for controlling horizontal movement
        private const float MaxMoveSpeed = 20f;
        private const float DragFactor = 0.75f;

        // Constants for controlling vertical movement
        private const float JumpLaunchVelocity = -60f;
        private const float GravityAcceleration = 9.81f;
        private const float MaxFallSpeed = 30f;

        // Inputs
        public Inputs Inputs
        {
            get
            {
                return _inputs;
            }
            set
            {
                OldInputs = _inputs;
                _inputs = value;
            }
        }
        public Inputs OldInputs;
        protected Inputs _inputs = new Inputs();

        public Player(int _index, string _pseudo, Vector2 _position) : base(_position, new Vector2(32, 32))
        {
            Index = _index;
            Pseudo = _pseudo;
        }

        public virtual void Update()
        {
            float Deltatime = GetDeltaTime();
            GetInputs();

            float movement = 0f;
            if (Inputs.Left) movement -= 1;
            if (Inputs.Right) movement += 1;

            // X Velocity
            Velocity.X += movement * Speed * Deltatime;
            Velocity.X *= DragFactor;
            Velocity.X = MathHelper.Clamp(Velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Y Velocity
            if (!IsGrounded)
            {
                Velocity.Y += GravityAcceleration * Deltatime;
                Velocity.Y = MathHelper.Clamp(Velocity.Y, -MaxFallSpeed, MaxFallSpeed);
            }
            else
            {
                Velocity.Y = 0;
            }
            HandleJump();
            HandleCollision();

            Move(Velocity);

            // Reset velocity if not moving
            if (MathF.Abs(Velocity.X) <= 2f) Velocity.X = 0;
            if (MathF.Abs(Velocity.Y) <= 0.1f) Velocity.Y = 0;
        }
        public override byte[] GetPacket()
        {
            PacketBuffer buffer = new PacketBuffer();
            // Base
            buffer.AddBytes(base.GetPacket());
            // State
            buffer.AddBool(IsGrounded);
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
            IsGrounded = buffer.GetBool();
            buffer.Dispose();
        }
        public virtual float GetDeltaTime()
        {
            return 0f;
        }
        public virtual void GetInputs()
        {
            Inputs = new Inputs();
        }
        public virtual void Dispose()
        {
            return;
        }
        public override void Move(Vector2 _force)
        {
            base.Move(_force);
        }
        public void HandleJump()
        {
            IsJumping = Inputs.Jump;

            if (IsGrounded && IsJumping)
            {
                Velocity.Y = JumpLaunchVelocity;
            }
        }
        public void HandleCollision()
        {
            GridPos = Grid.GetPos(Position);

            CollisionTop();
            CollisionBottom();
            CollisionLeft();
            CollisionRight();
        }
        public void CollisionTop()
        {
            Tile topLeft = Grid.I.GetTile(new Point(GridPos.X - 1, GridPos.Y - 1));
            Tile topCenter = Grid.I.GetTile(new Point(GridPos.X, GridPos.Y - 1));
            Tile topRight = Grid.I.GetTile(new Point(GridPos.X + 1, GridPos.Y - 1));

            Rectangle playerBounds = Bounds;

            if (topLeft != null && playerBounds.Left < topLeft.Bounds.Left && playerBounds.Top + Velocity.Y < topLeft.Bounds.Bottom)
            {
                if (topLeft.CollisionType == CollisionType.Impassable)
                {
                    Velocity.Y = topLeft.Bounds.Bottom - playerBounds.Top;
                }
            }

            if (topCenter != null && playerBounds.Top + Velocity.Y < topCenter.Bounds.Bottom)
            {
                if (topCenter.CollisionType == CollisionType.Impassable)
                {
                    Velocity.Y = topCenter.Bounds.Bottom - playerBounds.Top;
                }
            }

            if (topRight != null && playerBounds.Right > topRight.Bounds.Right && playerBounds.Top + Velocity.Y < topRight.Bounds.Bottom)
            {
                if (topRight.CollisionType == CollisionType.Impassable)
                {
                    Velocity.Y = topRight.Bounds.Bottom - playerBounds.Top;
                }
            }
        }
        public void CollisionBottom()
        {
            Tile bottomLeft = Grid.I.GetTile(new Point(GridPos.X - 1, GridPos.Y + 1));
            Tile bottomCenter = Grid.I.GetTile(new Point(GridPos.X, GridPos.Y + 1));
            Tile bottomRight = Grid.I.GetTile(new Point(GridPos.X + 1, GridPos.Y + 1));

            Rectangle playerBounds = Bounds;

            if (bottomLeft != null && playerBounds.Left < bottomLeft.Bounds.Left && playerBounds.Bottom + Velocity.Y > bottomLeft.Bounds.Top)
            {
                if (bottomLeft.CollisionType != CollisionType.Passable)
                {
                    Velocity.Y = bottomLeft.Bounds.Top - playerBounds.Bottom;
                }
            }

            if (bottomCenter != null && playerBounds.Bottom + Velocity.Y > bottomCenter.Bounds.Top)
            {
                if (bottomCenter.CollisionType != CollisionType.Passable)
                {
                    Velocity.Y = bottomCenter.Bounds.Top - playerBounds.Bottom;
                }
            }

            if (bottomRight != null && playerBounds.Right > bottomRight.Bounds.Right && playerBounds.Bottom + Velocity.Y > bottomRight.Bounds.Top)
            {
                if (bottomRight.CollisionType != CollisionType.Passable)
                {
                    Velocity.Y = bottomRight.Bounds.Top - playerBounds.Bottom;
                }
            }

            // Test grounded
            IsGrounded = (bottomLeft != null && bottomLeft.Bounds.Top <= playerBounds.Bottom + Velocity.Y) ||
                         (bottomCenter != null && bottomCenter.Bounds.Top <= playerBounds.Bottom + Velocity.Y) ||
                         (bottomRight != null && bottomRight.Bounds.Top <= playerBounds.Bottom + Velocity.Y);
        }
        public void CollisionLeft()
        {
            Tile leftCenter = Grid.I.GetTile(new Point(GridPos.X - 1, GridPos.Y));

            Rectangle playerBounds = Bounds;

            if (leftCenter != null && playerBounds.Left + Velocity.X < leftCenter.Bounds.Right)
            {
                if (leftCenter.CollisionType == CollisionType.Impassable)
                {
                    Velocity.X = leftCenter.Bounds.Right - playerBounds.Left;
                    Console.WriteLine(string.Format("Vel : {0}, player left : {1}, tile right : {2}", Velocity.ToString(), playerBounds.Left, leftCenter.Bounds.Right));
                }
            }
        }
        public void CollisionRight()
        {
            Tile rightCenter = Grid.I.GetTile(new Point(GridPos.X + 1, GridPos.Y));

            Rectangle playerBounds = Bounds;

            if (rightCenter != null && playerBounds.Right + Velocity.X > rightCenter.Bounds.Left)
            {
                if (rightCenter.CollisionType == CollisionType.Impassable)
                {
                    Velocity.X = rightCenter.Bounds.Left - playerBounds.Right;
                    Console.WriteLine(string.Format("Vel : {0}, player right : {1}, tile left : {2}", Velocity.ToString(), playerBounds.Right, rightCenter.Bounds.Left));
                }
            }
        }
    }
}