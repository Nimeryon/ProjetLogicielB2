using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    public class Sprite : Renderable
    {
        public Texture2D Texture;
        public Rectangle? Rectangle;
        public Point Size;

        // Constructors
        public Sprite(Vector2 _position)
        {
            Position = _position;
        }
        public Sprite(Texture2D _texture, Vector2 _position)
        {
            Texture = _texture;
            Size = Texture.Bounds.Size;
            Position = _position;
        }
        public Sprite(Texture2D _texture, Vector2 _position, Alignment _alignment)
        {
            Texture = _texture;
            Size = Texture.Bounds.Size;
            Position = _position;
            Alignment = _alignment;
        }
        public Sprite(Texture2D _texture, Vector2 _position, Vector2 _oringNormalized)
        {
            Texture = _texture;
            Size = Texture.Bounds.Size;
            Position = _position;
            Origin = _oringNormalized;
        }

        public void SetTexture(Texture2D _texture)
        {
            Texture = _texture;
            Size = Texture.Bounds.Size;
        }
        public void SetAlignment(Alignment _alignment)
        {
            Alignment = _alignment;
        }
        public void SetFromSpriteSheet(Point _index, Point _spritesCount)
        {
            Point _size = Texture.Bounds.Size / _spritesCount;
            Size = _size;
            Rectangle = new Rectangle(_index * _size, _size);
        }
        public void SetRectangle(Rectangle _rectangle)
        {
            Rectangle = _rectangle;
        }
        public override Vector2 GetSize()
        {
            return Size.ToVector2();
        }
        public override void Draw()
        {
            if (Texture == null) return;

            Rectangle _srcRect = Rectangle ?? new Rectangle(0, 0, Texture.Width, Texture.Height);
            GameHandler.SpriteBatch.Draw(Texture, WorldTransfrom.Position, _srcRect, WorldTransfrom.Color, WorldTransfrom.Rotation, WorldTransfrom.Origin, WorldTransfrom.Scale, Effects, ZIndex);
        }
    }
}
