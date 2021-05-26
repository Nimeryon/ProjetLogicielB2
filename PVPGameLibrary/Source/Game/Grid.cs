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
            TileGrid = new Tile[96 / 2, 48 / 2];
            LoadLayout();
        }
        public void LoadLayout()
        {
            // Terrain
            for (int x = 1; x < TileGrid.GetLength(0) - 1; x++)
            {
                SetTile(new Tile(TerrainType.Wood, new Point(x, 0)));
                SetTile(new Tile(TerrainType.Wood, new Point(x, 1)));

                SetTile(new Tile(TerrainType.Stone, new Point(x, 22)));
                SetTile(new Tile(TerrainType.Stone, new Point(x, 23)));
            }

            for (int y = 0; y < TileGrid.GetLength(1); y++)
            {
                if (y > 16)
                {
                    SetTile(new Tile(TerrainType.Stone, new Point(0, y)));
                    SetTile(new Tile(TerrainType.Stone, new Point(1, y)));

                    SetTile(new Tile(TerrainType.Stone, new Point(46, y)));
                    SetTile(new Tile(TerrainType.Stone, new Point(47, y)));
                }
                else
                {
                    SetTile(new Tile(TerrainType.Wood, new Point(0, y)));
                    SetTile(new Tile(TerrainType.Wood, new Point(1, y)));

                    SetTile(new Tile(TerrainType.Wood, new Point(46, y)));
                    SetTile(new Tile(TerrainType.Wood, new Point(47, y)));
                }
            }

            SetTile(new Tile(TerrainType.Wood, new Point(2, 2)));
            SetTile(new Tile(TerrainType.Wood, new Point(45, 2)));
            SetTile(new Tile(TerrainType.Stone, new Point(2, 21)));
            SetTile(new Tile(TerrainType.Stone, new Point(45, 21)));

            TerrainPatch(new Point(9, 3), new Point(24 - 4, 19), TerrainType.GreenGrass);

            PlatformPatch(3, new Point(24 - 4 - 3, 19), PlatformType.Wood);
            TerrainPatch(new Point(2, 1), new Point(7, 16), TerrainType.YellowGrass, CollisionType.BottomPassable);
            TerrainPatch(new Point(2, 5), new Point(7, 17), TerrainType.YellowGrass, CollisionType.Passable);
            WallPatch(new Point(2, 2), new Point(9, 20), WallType.Iron);
            PlatformPatch(5, new Point(41, 17), PlatformType.Stone);

            PlatformPatch(3, new Point(24 + 5, 19), PlatformType.Wood);
            TerrainPatch(new Point(2, 1), new Point(39, 16), TerrainType.YellowGrass, CollisionType.BottomPassable);
            TerrainPatch(new Point(2, 5), new Point(39, 17), TerrainType.YellowGrass, CollisionType.Passable);
            WallPatch(new Point(2, 2), new Point(37, 20), WallType.Iron);
            PlatformPatch(5, new Point(2, 17), PlatformType.Stone);
        }
        public void TerrainPatch(Point size, Point pos, TerrainType type, CollisionType collision = CollisionType.Impassable)
        {
            for (int y = pos.Y; y < pos.Y + size.Y; y++)
            {
                for (int x = pos.X; x < pos.X + size.X; x++)
                {
                    SetTile(new Tile(type, new Point(x, y), collision));
                }
            }
        }
        public void PlatformPatch(int length, Point pos, PlatformType type)
        {
            for (int x = pos.X; x < pos.X + length; x++)
            {
                SetTile(new Tile(type, new Point(x, pos.Y)));
            }
        }
        public void WallPatch(Point size, Point pos, WallType type)
        {
            for (int y = pos.Y; y < pos.Y + size.Y; y++)
            {
                for (int x = pos.X; x < pos.X + size.X; x++)
                {
                    SetTile(new Tile(type, new Point(x, y)));
                }
            }
        }
        public void SetTile(Tile tile)
        {
            TileGrid[tile.GridPos.X, tile.GridPos.Y] = tile;
        }
        public Tile GetTile(Point gridPos)
        {
            try
            {
                return TileGrid[gridPos.X, gridPos.Y];
            }
            catch
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