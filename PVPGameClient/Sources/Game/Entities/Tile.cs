using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using PVPGameLibrary;

namespace PVPGameClient
{
    public enum PlatformType
    {
        Stone = 0,
        Wood = 1,
        Gold = 2
    }
    public enum TerrainType
    {
        Stone = 0,
        Wood = 1,
        Scale = 2,
        Brick = 3,
        GreenGrass = 4,
        YellowGrass = 5,
        PinkGrass = 6
    }
    public enum WallType
    {
        Bronze = 0,
        Iron = 1,
        Copper = 2,
        Gold = 3
    }

    public class Tile : PVPGameLibrary.Tile
    {
        public Sprite Sprite;

        public Tile(PlatformType type, Vector2 gridPos) : base(TileType.Platform, CollisionType.Passable, gridPos)
        {
            Sprite = new Sprite(Loader.Platforms[(int)type], Position);
            Sprite.Scale = new Vector2(2, 2);
            Sprite.SetFromSpriteSheet(new Point(1, 0), new Point(3, 1));
        }
        public Tile(TerrainType type, Vector2 gridPos) : base(TileType.Terrain, CollisionType.Impassable, gridPos)
        {
            Sprite = new Sprite(Loader.Terrains[(int)type], Position);
            Sprite.Scale = new Vector2(2, 2);
            Sprite.SetFromSpriteSheet(new Point(1, 0), new Point(5, 3));
        }
        public Tile(WallType type, Vector2 gridPos) : base(TileType.Wall, CollisionType.Impassable, gridPos)
        {
            Sprite = new Sprite(Loader.Walls[(int)type], Position);
            Sprite.Scale = new Vector2(2, 2);
            Sprite.SetFromSpriteSheet(new Point(0, 1), new Point(4, 3));
        }
    }
}
