using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    class Text : Renderable
    {
        public string TextString;
        public SpriteFont Font;

        // Setters
        public void SetText(string _text)
        {
            TextString = _text;
        }
        public void SetFont(SpriteFont _font)
        {
            Font = _font;
        }

        // Constructors
        public Text(SpriteFont _font, string _text, Vector2 _position)
        {
            TextString = _text;
            Font = _font;
            Position = _position;
        }
        public Text(SpriteFont _font, string _text, Vector2 _position, Alignment _alignment)
        {
            TextString = _text;
            Font = _font;
            Position = _position;
            Alignment = _alignment;
        }
        public Text(SpriteFont _font, string _text, Vector2 _position, Vector2 _oringNormalized)
        {
            TextString = _text;
            Font = _font;
            Position = _position;
            Origin = _oringNormalized;
        }

        public override Vector2 GetSize()
        {
            return Font.MeasureString(TextString);
        }

        // Functions
        public override void Draw()
        {
            GameHandler.SpriteBatch.DrawString(Font, TextString, WorldTransfrom.Position, WorldTransfrom.Color, WorldTransfrom.Rotation, WorldTransfrom.Origin, WorldTransfrom.Scale, Effects, ZIndex);
        }
    }
}
