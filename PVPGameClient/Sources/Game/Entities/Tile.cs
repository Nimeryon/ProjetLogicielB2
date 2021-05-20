using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    class Tile
    {
        public Sprite Sprite;

        public Tile(Sprite sprite)
        {
            Sprite = sprite;
        }
        public Tile(Sprite sprite, Point spriteIndex, Point spriteCount)
        {
            Sprite = sprite;
            Sprite.SetFromSpriteSheet(spriteIndex, spriteCount);
        }
    }
}
