using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PVPGameLibrary;
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
        public Vector2 Direction = new Vector2(Helpers.RandomRange(-2f, 2f), Helpers.RandomRange(-2f, 2f));

        public BackGround(BackGroundColor _color) : base(new Vector2(-64, -64))
        {
            BackGroundColor = _color;
            SetTexture(Loader.BackGrounds[Helpers.RandomRange(0, 6)]);
            SetRectangle(new Rectangle(0, 0, GameHandler.Viewport.Width + 128, GameHandler.Viewport.Height + 128));
        }

        public override void Update()
        {
            Move(Direction * Globals.DeltaTime10);
            if (Position.X >= 0 || Position.X <= -128)
            {
                Position = new Vector2(-64, Position.Y);
            }
            if (Position.Y >= 0 || Position.Y <= -128)
            {
                Position = new Vector2(Position.X, -64);
            }
        }
    }
}
