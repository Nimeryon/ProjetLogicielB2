using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using PVPGameLibrary;

namespace PVPGameClient
{
    public class Player : PVPGameLibrary.Player
    {
        public bool IsCurrentPlayer = true;

        private AnimationPlayer _animationPlayer;

        private Animation FallAnimation;
        private Animation JumpAnimation;
        private Animation IdleAnimation;
        private Animation RunAnimation;

        public Player(int _index, string _pseudo, PlayerCharacter _character, Texture2D _texture, Vector2 _position, bool _currentPlayer = true) : base(_index, _pseudo, _character, _position)
        {
            IsCurrentPlayer = _currentPlayer;

            switch (Character)
            {
                case PlayerCharacter.Frog:
                    FallAnimation = new Animation(Loader.Frog[0], 1, true);
                    JumpAnimation = new Animation(Loader.Frog[1], 1, true);
                    IdleAnimation = new Animation(Loader.Frog[2], 11, true);
                    RunAnimation = new Animation(Loader.Frog[3], 12, true);
                    break;
                case PlayerCharacter.Mask:
                    FallAnimation = new Animation(Loader.Mask[0], 1, true);
                    JumpAnimation = new Animation(Loader.Mask[1], 1, true);
                    IdleAnimation = new Animation(Loader.Mask[2], 11, true);
                    RunAnimation = new Animation(Loader.Mask[3], 12, true);
                    break;
                case PlayerCharacter.Pink:
                    FallAnimation = new Animation(Loader.Pink[0], 1, true);
                    JumpAnimation = new Animation(Loader.Pink[1], 1, true);
                    IdleAnimation = new Animation(Loader.Pink[2], 11, true);
                    RunAnimation = new Animation(Loader.Pink[3], 12, true);
                    break;
                case PlayerCharacter.Virtual:
                    FallAnimation = new Animation(Loader.Virtual[0], 1, true);
                    JumpAnimation = new Animation(Loader.Virtual[1], 1, true);
                    IdleAnimation = new Animation(Loader.Virtual[2], 11, true);
                    RunAnimation = new Animation(Loader.Virtual[3], 12, true);
                    break;
            }

            _animationPlayer = new AnimationPlayer(new Sprite(IdleAnimation.Texture, Position), IdleAnimation);
            if (IsCurrentPlayer) GameHandler.OnUpdate += Update;
        }

        public override void Update()
        {
            Animate();
            if (!IsCurrentPlayer) return;

            // Change behavior of client predict
            //base.Update();
            GetInputs();

            if (InputSystem.GetKeyDown(Keys.F4)) _animationPlayer.Sprite.DebugRectangle = !_animationPlayer.Sprite.DebugRectangle;

            if (Inputs.SameAs(OldInputs))
            {
                GameHandler.ClienTCP.SendState(Inputs);
                Console.WriteLine(string.Format("Left:{0} / Right:{1} / Jump:{2} / Attack:{3}", Inputs.Left, Inputs.Right, Inputs.Jump, Inputs.Attack));
            }
        }
        public void Animate()
        {
            if (Velocity.Y > 0) _animationPlayer.PlayAnimation(JumpAnimation);
            else if (Velocity.Y < 0) _animationPlayer.PlayAnimation(FallAnimation);
            else if (Math.Abs(Velocity.X) > 0) _animationPlayer.PlayAnimation(RunAnimation);
            else _animationPlayer.PlayAnimation(IdleAnimation);
        }
        public override void LoadPacket(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            MoveAt(new Vector2(buffer.GetFloat(), buffer.GetFloat()));
            Scale = new Vector2(buffer.GetFloat(), buffer.GetFloat());
            Rotation = buffer.GetFloat();
            Velocity = new Vector2(buffer.GetFloat(), buffer.GetFloat());
            IsGrounded = buffer.GetBool();
            buffer.Dispose();
        }
        public override float GetDeltaTime()
        {
            return Globals.DeltaTime10;
        }
        public override void GetInputs()
        {
            Inputs = new Inputs(
                InputSystem.GetKey(Keys.Q),
                InputSystem.GetKey(Keys.D),
                InputSystem.GetKey(Keys.Space),
                InputSystem.GetMouseKey(0)
            );
        }
        public override void Move(Vector2 _force)
        {
            _animationPlayer.Sprite.Position += _force;
            base.Move(_force);
        }
        public override void MoveAt(Vector2 _position)
        {
            _animationPlayer.Sprite.Position = _position;
            base.MoveAt(_position);
        }
        public override void Dispose()
        {
            if (IsCurrentPlayer)
            {
                GameHandler.OnUpdate -= Update;
            }
            _animationPlayer.Sprite.Dispose();
            _animationPlayer.Dispose();
        }
    }
}