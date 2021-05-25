using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameLibrary
{
    public enum TileType
    {
        Platform,
        Terrain,
        Wall
    }
    public enum CollisionType
    {
        Passable,
        BottomPassable,
        Impassable
    }

    public class Tile : Transform
    {
        public TileType Type;
        public CollisionType CollisionType;
        public Vector2 GridPos;

        public Tile(TileType type, CollisionType collisionType, Vector2 gridPos)
        {
            Type = type;
            CollisionType = collisionType;
            GridPos = gridPos;

            // Set Transform properties
            Scale = new Vector2(2, 2);
            switch(Type)
            {
                case TileType.Platform:
                    Size = new Vector2(16, 5);
                    break;
                default:
                    Size = new Vector2(16, 16);
                    break;
            }
            Position = GridPos * (new Vector2(16, 16) * Scale);
            Console.WriteLine(string.Format("{0} / {1} / {2}", Bounds.ToString(), Position.ToString(), GridPos.ToString()));
        }
    }
}