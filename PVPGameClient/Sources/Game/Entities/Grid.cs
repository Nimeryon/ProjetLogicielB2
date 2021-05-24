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
            for (int x = 0; x < TileGrid.GetLength(0); x++)
            {
                Point pos = new Point(x, TileGrid.GetLength(1) - 1);
                SetTile(pos, new Tile(TerrainType.Stone, pos.ToVector2()));
            }
        }
    }
}
