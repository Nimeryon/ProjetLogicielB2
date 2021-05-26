using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    public class VisualGrid
    {
        public static VisualGrid I;
        public VisualTile[,] TileGrid;

        public VisualGrid()
        {
            I = this;
            TileGrid = new VisualTile[64 / 2, 48 / 2];
            for (int y = 0; y < TileGrid.GetLength(1); y++)
            {
                for (int x = 0; x < TileGrid.GetLength(0); x++)
                {
                    TileGrid[x, y] = new VisualTile(new Point(x, y));
                }
            }
        }
        public VisualTile GetTile(Point gridPos)
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
    }
}
