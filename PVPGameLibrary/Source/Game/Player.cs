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
        private const float DragFactor = 0.9f;

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

        public Player(int _index, string _pseudo, Vector2 _position) : base(_position)
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

            Move(Velocity);

            // Test collisions
            HandleCollision();

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
            IsGrounded = false;


            int leftTile = GridPos.X;
            int rightTile = GridPos.X - 1;
            int topTile = GridPos.Y;
            int bottomTile = GridPos.Y - 1;

            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    Tile tile = Grid.I.GetTile(new Point(x, y));
                    if (tile == null) continue;

                    if (tile.CollisionType != CollisionType.Passable)
                    {
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(Bounds, tile.Bounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            // Resolve the collision along the shallow axis.
                            if (absDepthY < absDepthX || tile.Type == TileType.Platform)
                            {
                                if (PreviousBottom <= tile.Bounds.Top)
                                {
                                    IsGrounded = true;
                                    Console.WriteLine("On Ground");
                                }

                                if (tile.CollisionType == CollisionType.Impassable || IsGrounded)
                                {
                                    MoveAt(new Vector2(Position.X, Position.Y + depth.Y));
                                }
                            }
                            else if (tile.CollisionType == CollisionType.Impassable)
                            {
                                MoveAt(new Vector2(Position.X + depth.X, Position.Y));
                            }
                        }
                    }
                }
            }

            PreviousBottom = Bounds.Bottom;
        }
    }
}