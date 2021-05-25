using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameLibrary
{
    public class Grid
    {
        public static Grid I;
        public static Vector2 Size = new Vector2(32, 32);
        public Tile[,] TileGrid;

        public Grid()
        {
            I = this;
            Load();
        }

        public void Load()
        {
            TileGrid = new Tile[96 / 2, 48 / 2];
            LoadLayout();
        }
        public virtual void LoadLayout()
        {
            Point pos;
            for (int x = 1; x < TileGrid.GetLength(0) - 1; x++)
            {
                pos = new Point(x, TileGrid.GetLength(1) - 1);
                SetTile(pos, new Tile(TileType.Terrain, CollisionType.Impassable,pos.ToVector2()));

                pos = new Point(x, TileGrid.GetLength(1) - 5);
                SetTile(pos, new Tile(TileType.Platform, CollisionType.BottomPassable, pos.ToVector2()));

                pos = new Point(x, TileGrid.GetLength(1) - 10);
                SetTile(pos, new Tile(TileType.Platform, CollisionType.BottomPassable, pos.ToVector2()));

                pos = new Point(x, TileGrid.GetLength(1) - 15);
                SetTile(pos, new Tile(TileType.Platform, CollisionType.BottomPassable, pos.ToVector2()));
            }

            for (int y = 0; y < TileGrid.GetLength(1); y++)
            {
                pos = new Point(0, y);
                SetTile(pos, new Tile(TileType.Terrain, CollisionType.Impassable, pos.ToVector2()));

                pos = new Point(TileGrid.GetLength(0) - 1, y);
                SetTile(pos, new Tile(TileType.Terrain, CollisionType.Impassable, pos.ToVector2()));
            }

            pos = new Point(24, TileGrid.GetLength(1) - 16);
            SetTile(pos, new Tile(TileType.Wall, CollisionType.Impassable, pos.ToVector2()));
        }
        public void SetTile(Point pos, Tile tile)
        {
            TileGrid[pos.X, pos.Y] = tile;
        }
        public Tile GetTile(Point gridPos)
        {
            try
            {
                return TileGrid[gridPos.X, gridPos.Y];
            }
            catch (IndexOutOfRangeException e)
            {
                return null;
            }
        }
        public static Point GetPos(Vector2 pos)
        {
            float X = pos.X / Size.X;
            float Y = pos.Y / Size.Y;
            return new Point((int)MathF.Floor(X), (int)MathF.Floor(Y));
        }
    }
}