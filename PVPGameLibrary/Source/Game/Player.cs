using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameLibrary
{
    public enum PlayerCharacter
    {
        Frog,
        Mask,
        Pink,
        Virtual
    }

    public class Player : Entity, IDisposable
    {
        // Properties
        public int Index;
        public string Pseudo;
        public PlayerCharacter Character;

        public bool IsGrounded;
        public bool IsJumping;
        public Point GridPos;
        public float PreviousBottom;

        // Constants for controlling horizontal movement
        private const float MaxMoveSpeed = 20f;
        private const float DragFactor = 0.75f;

        // Constants for controlling vertical movement
        private const float JumpLaunchVelocity = -50f;
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

        public Player(int _index, string _pseudo, PlayerCharacter _character, Vector2 _position) : base(_position, new Vector2(32, 32))
        {
            Index = _index;
            Pseudo = _pseudo;
            Character = _character;
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
            Tile topCenter = Grid.I.GetTile(new Point(GridPos.X, GridPos.Y - 1));

            Rectangle playerBounds = Bounds;

            if (topCenter != null && playerBounds.Top + Velocity.Y < topCenter.Bounds.Bottom)
            {
                if (topCenter.CollisionType == CollisionType.Impassable)
                {
                    Velocity.Y = topCenter.Bounds.Bottom - playerBounds.Top;
                }
            }
        }
        public void CollisionBottom()
        {
            Tile bottomCenter = Grid.I.GetTile(new Point(GridPos.X, GridPos.Y + 1));

            Rectangle playerBounds = Bounds;

            if (bottomCenter != null && playerBounds.Bottom + Velocity.Y > bottomCenter.Bounds.Top)
            {
                if (bottomCenter.CollisionType != CollisionType.Passable && !IsGrounded)
                {
                    Velocity.Y = 0;
                    MoveAt(new Vector2(Position.X, bottomCenter.Bounds.Top - 32));
                }
            }

            // Test grounded
            IsGrounded = bottomCenter != null && bottomCenter.Bounds.Top <= playerBounds.Bottom + Velocity.Y && bottomCenter.CollisionType != CollisionType.Passable;
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
                }
            }
        }
    }
}