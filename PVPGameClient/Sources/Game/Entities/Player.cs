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

            _sprite = new Sprite(_texture, Position, Alignment.MiddleCenter);
            _sprite.SetFromSpriteSheet(new Point(1, 0), new Point(3, 4));
            if (IsCurrentPlayer) GameHandler.OnUpdate += Update;
        }

        public override void Update()
        {
            if (!IsCurrentPlayer) return;

            base.Update();

            if (Inputs.SameAs(OldInputs))
            {
                GameHandler.ClienTCP.SendState(Inputs);
                Console.WriteLine(string.Format("Left:{0} / Right:{1} / Jump:{2} / Attack:{3}", Inputs.Left, Inputs.Right, Inputs.Jump, Inputs.Attack));
            }
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