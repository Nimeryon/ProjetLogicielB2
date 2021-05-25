using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using PVPGameLibrary;

namespace PVPGameClient
{
    public class Grid : PVPGameLibrary.Grid
    {
        public override void LoadLayout()
        {
            Point pos;
            for (int x = 1; x < TileGrid.GetLength(0) - 1; x++)
            {
                pos = new Point(x, TileGrid.GetLength(1) - 1);
                SetTile(pos, new Tile(TerrainType.Stone, pos.ToVector2()));

                pos = new Point(x, TileGrid.GetLength(1) - 5);
                SetTile(pos, new Tile(PlatformType.Wood, pos.ToVector2()));

                pos = new Point(x, TileGrid.GetLength(1) - 10);
                SetTile(pos, new Tile(PlatformType.Wood, pos.ToVector2()));

                pos = new Point(x, TileGrid.GetLength(1) - 15);
                SetTile(pos, new Tile(PlatformType.Wood, pos.ToVector2()));
            }

            for (int y = 0; y < TileGrid.GetLength(1); y++)
            {
                pos = new Point(0, y);
                SetTile(pos, new Tile(TerrainType.Brick, pos.ToVector2()));

                pos = new Point(TileGrid.GetLength(0) - 1, y);
                SetTile(pos, new Tile(TerrainType.Brick, pos.ToVector2()));
            }

            pos = new Point(24, TileGrid.GetLength(1) - 16);
            SetTile(pos, new Tile(WallType.Copper, pos.ToVector2()));
        }
    }
}
