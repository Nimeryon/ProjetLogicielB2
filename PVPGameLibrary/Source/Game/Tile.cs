using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameLibrary
{
    public enum CollisionType
    {
        Passable,
        BottomPassable,
        Impassable
    }
    public enum TileType
    {
        Platform,
        Terrain,
        Wall
    }
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

    public class Tile : Transform
    {
        public CollisionType CollisionType;
        public Point GridPos;
        public TileType Type;

        public PlatformType? PlatformType;
        public TerrainType? TerrainType;
        public WallType? WallType;

        public Tile(PlatformType type, Point gridPos, CollisionType collision = CollisionType.BottomPassable)
        {
            Type = TileType.Platform;
            PlatformType = type;
            CollisionType = collision;
            GridPos = gridPos;
            Size = new Vector2(16, 5);
            Initialise();
        }
        public Tile(TerrainType type, Point gridPos, CollisionType collision = CollisionType.Impassable)
        {
            Type = TileType.Terrain;
            TerrainType = type;
            CollisionType = collision;
            GridPos = gridPos;
            Size = new Vector2(16, 16);
            Initialise();
        }
        public Tile(WallType type, Point gridPos, CollisionType collision = CollisionType.Impassable)
        {
            Type = TileType.Wall;
            WallType = type;
            CollisionType = collision;
            GridPos = gridPos;
            Size = new Vector2(16, 16);
            Initialise();
        }
        public virtual void Initialise()
        {
            // Set Transform properties
            Scale = new Vector2(2, 2);
            Position = GridPos.ToVector2() * (new Vector2(16, 16) * Scale);
        }
    }
}