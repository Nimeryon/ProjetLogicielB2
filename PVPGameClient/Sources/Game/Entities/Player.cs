using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PVPGameClient
{
    public class Player : Entity, IDisposable
    {
        public bool IsCurrentPlayer = true;
        public int Index;
        public PlayerState CurrentState = new PlayerState();
        public PlayerState OldState;

        private Sprite _sprite;

        public Player(int _index, Texture2D _texture, Vector2 _position, bool _currentPlayer = true)
        {
            IsCurrentPlayer = _currentPlayer;
            Index = _index;
            _sprite = new Sprite(_texture, _position, Alignment.MiddleCenter);
            _sprite.SetFromSpriteSheet(new Point(1, 0), new Point(3, 4));
            if (IsCurrentPlayer) GameHandler.OnUpdate += Update;
        }

        private void Update()
        {
            if (!IsCurrentPlayer) return;

            OldState = CurrentState;
            CurrentState = new PlayerState();

            Vector2 movement = Vector2.Zero;
            if (CurrentState.Up) movement.Y -= 1;
            if (CurrentState.Down) movement.Y += 1;
            if (CurrentState.Left) movement.X -= 1;
            if (CurrentState.Right) movement.X += 1;

            if (movement != Vector2.Zero) _sprite.Move(Vector2.Normalize(movement) * Speed * Globals.DeltaTime10);

            if (CurrentState.SameAs(OldState))
            {
                GameHandler.ClienTCP.SendState(CurrentState);
                Console.WriteLine(string.Format("Up:{0} / Down:{1} / Left:{2} / Right:{3}", CurrentState.Up, CurrentState.Down, CurrentState.Left, CurrentState.Right));
            }
        }
        public void Move(Vector2 pos)
        {
            _sprite.Position = pos;
        }
        public void Dispose()
        {
            if (IsCurrentPlayer)
            {
                GameHandler.OnUpdate -= Update;
            }
            _sprite.Dispose();
        }
    }
}