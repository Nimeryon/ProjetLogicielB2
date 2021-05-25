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

        private Sprite _sprite;

        public Player(int _index, string _pseudo, Texture2D _texture, Vector2 _position, bool _currentPlayer = true) : base(_index, _pseudo, _position)
        {
            IsCurrentPlayer = _currentPlayer;

            _sprite = new Sprite(_texture, Position);
            _sprite.SetFromSpriteSheet(new Point(1, 0), new Point(3, 4));
            _sprite.DebugRectangle = true;
            if (IsCurrentPlayer) GameHandler.OnUpdate += Update;
        }

        public override void Update()
        {
            if (!IsCurrentPlayer) return;

            // Change behavior of client predict
            //base.Update();
            GetInputs();

            if (InputSystem.GetKeyDown(Keys.F4)) _sprite.DebugRectangle = !_sprite.DebugRectangle;

            if (Inputs.SameAs(OldInputs))
            {
                GameHandler.ClienTCP.SendState(Inputs);
                Console.WriteLine(string.Format("Left:{0} / Right:{1} / Jump:{2} / Attack:{3}", Inputs.Left, Inputs.Right, Inputs.Jump, Inputs.Attack));
            }
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
            _sprite.Position += _force;
            base.Move(_force);
        }
        public override void MoveAt(Vector2 _position)
        {
            _sprite.Position = _position;
            base.MoveAt(_position);
        }
        public override void Dispose()
        {
            if (IsCurrentPlayer)
            {
                GameHandler.OnUpdate -= Update;
            }
            _sprite.Dispose();
        }
    }
}