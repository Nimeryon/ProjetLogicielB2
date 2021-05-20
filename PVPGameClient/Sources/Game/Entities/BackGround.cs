using Bindings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    public enum BackGroundColor
    {
        Blue = 0,
        Brown = 1,
        Gray = 2,
        Green = 3,
        Pink = 4,
        Purple = 5,
        Yellow = 6
    }

    public class BackGround : Sprite
    {
        public BackGroundColor BackGroundColor;
        public Vector2 Direction = new Vector2(-1, -1);

        public BackGround(BackGroundColor _color) : base(Vector2.Zero)
        {
            BackGroundColor = _color;
            SetTexture(Loader.BackGrounds[Helpers.RandomRange(0, 6)]);
            SetRectangle(new Rectangle(0, 0, GameHandler.Viewport.Width + 64, GameHandler.Viewport.Height + 64));
        }

        public override void Update()
        {
            Move(Direction * Globals.DeltaTime10);
            if (Position.X >= 64 || Position.X <= -64)
            {
                Position = new Vector2(0, Position.Y);
            }
            if (Position.Y >= 64 || Position.Y <= -64)
            {
                Position = new Vector2(Position.X, 0);
            }
        }
    }
}
