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
        public bool CanJump;
        public float JumpTime;
        
        // Constants for controlling horizontal movement
        private const float MaxMoveSpeed = 20f;
        private const float GroundDragFactor = 0.9f;
        private const float AirDragFactor = 0.8f;

        // Constants for controlling vertical movement
        private const float MaxJumpTime = 0.5f;
        private const float JumpLaunchVelocity = -100f;
        private const float GravityAcceleration = 10f;
        private const float MaxFallSpeed = 10f;
        private const float JumpControlPower = 0.2f;

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

            if (IsGrounded)
            {
                Velocity.X *= GroundDragFactor;
            }
            else 
            { 
                Velocity.X *= AirDragFactor; 
            }

            Velocity.X = MathHelper.Clamp(Velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Y Velocity
            if (!IsGrounded)
            {
                Velocity.Y += GravityAcceleration * Deltatime;
                Velocity.Y = MathHelper.Clamp(Velocity.Y, -MaxFallSpeed, MaxFallSpeed);
            }
            HandleJump();

            Move(Velocity);

            // Test collisions
            HandleCollision();

            // Reset velocity if not moving
            //if (Position.X == OldPosition.X) Velocity.X = 0;
            if (Position.Y == OldPosition.Y || IsGrounded) Velocity.Y = 0;
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
            float Deltatime = GetDeltaTime();

            if (Inputs.Jump && CanJump)
            {
                IsJumping = true;
                CanJump = false;
            }

            if (IsJumping)
            {
                if ((!IsJumping && IsGrounded) || JumpTime > 0.0f) JumpTime += Deltatime;

                if (JumpTime <= MaxJumpTime)
                {
                    Velocity.Y += JumpLaunchVelocity * (1.0f - (float)Math.Pow(JumpTime / MaxJumpTime, JumpControlPower)) * Deltatime;
                }
            }
        }
        public void HandleCollision()
        {
            if (Position.Y >= 500f)
            {
                IsGrounded = true;
                MoveAt(new Vector2(Position.X, 500f));

                if (!CanJump)
                {
                    CanJump = true;
                    IsJumping = false;
                }
            }
            else
            {
                IsGrounded = false;
            }
        }
    }
}