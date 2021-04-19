using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Bindings;

namespace PVPGameClient
{
    public class Player : Entity, IDisposable
    {
        public bool IsCurrentPlayer = true;
        private Sprite _sprite;

        public Player(Texture2D _texture, Vector2 _position, bool _currentPlayer = true)
        {
            IsCurrentPlayer = _currentPlayer;
            _sprite = new Sprite(_texture, _position, Alignment.MiddleCenter);
            _sprite.SetFromSpriteSheet(new Point(1, 0), new Point(3, 4));
            //if (IsCurrentPlayer) TickSystem.OnSmallTickEvent += UpdateServer;
            GameHandler.OnUpdate += Update;
        }

        private void UpdateServer()
        {
            GameHandler.ClienTCP.SendMovement(_sprite.WorldTransfrom.Position);
        }
        private void Update()
        {
            if (!IsCurrentPlayer) return;

            Vector2 movement = Vector2.Zero;
            if (InputSystem.GetKey(Keys.Z))
            {
                movement.Y -= 1;
            }
            if (InputSystem.GetKey(Keys.S))
            {
                movement.Y += 1;
            }
            if (InputSystem.GetKey(Keys.Q))
            {
                movement.X -= 1;
            }
            if (InputSystem.GetKey(Keys.D))
            {
                movement.X += 1;
            }

            if (movement != Vector2.Zero) _sprite.Move(Vector2.Normalize(movement) * Speed * Globals.DeltaTime10);
        }
        public void Dispose()
        {
            if (IsCurrentPlayer) TickSystem.OnSmallTickEvent -= UpdateServer;
            GameHandler.OnUpdate -= Update;
            _sprite.Dispose();
        }
    }
}
