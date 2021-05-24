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
            for (int x = 0; x < TileGrid.GetLength(0); x++)
            {
                Point pos = new Point(x, TileGrid.GetLength(1) - 1);
                SetTile(pos, new Tile(TileType.Terrain, CollisionType.Impassable,pos.ToVector2()));
            }
        }
        public void SetTile(Point pos, Tile tile)
        {
            TileGrid[pos.X, pos.Y] = tile;
        }
        public  Tile GetTile(Point gridPos)
        {
            return TileGrid[gridPos.X, gridPos.Y];
        }
        public static Point GetPos(Vector2 pos)
        {
            float X = pos.X / Size.X;
            float Y = pos.Y / Size.Y;
            return new Point((int)MathF.Floor(X), (int)MathF.Floor(Y));
        }
    }
}