using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using PVPGameLibrary;

namespace PVPGameClient
{
    public class VisualTile
    {
        public static Point[] PlatformPoints = new Point[]
        {
            new Point(1, 0), //0
            new Point(2, 0), //1
            new Point(1, 0), //2
            new Point(2, 0), //3
            new Point(0, 0), //4
            new Point(1, 0), //5
            new Point(0, 0), //6
            new Point(1, 0), //7
            new Point(1, 0), //8
            new Point(2, 0), //9
            new Point(1, 0), //10
            new Point(2, 0), //11
            new Point(0, 0), //12
            new Point(1, 0), //13
            new Point(0, 0), //14
            new Point(1, 0) //15
        };
        public static Point[] TerrainPoints = new Point[]
        {
            new Point(0, 0), //0
            new Point(0, 0), //1
            new Point(0, 0), //2
            new Point(2, 2), //3
            new Point(0, 0), //4
            new Point(0, 0), //5
            new Point(0, 2), //6
            new Point(1, 2), //7
            new Point(0, 0), //8
            new Point(2, 0), //9
            new Point(0, 0), //10
            new Point(2, 1), //11
            new Point(0, 0), //12
            new Point(1, 0), //13
            new Point(0, 1), //14
            new Point(1, 1) //15
        };
        public static Point[] WallPoints = new Point[]
        {
            new Point(0, 1), //0
            new Point(2, 0), //1
            new Point(3, 2), //2
            new Point(2, 2), //3
            new Point(0, 0), //4
            new Point(1, 0), //5
            new Point(1, 2), //6
            new Point(1, 0), //7
            new Point(3, 0), //8
            new Point(2, 1), //9
            new Point(3, 1), //10
            new Point(3, 1), //11
            new Point(1, 1), //12
            new Point(1, 0), //13
            new Point(3, 1), //14
            new Point(0, 1) //15
        };

        public Tile Tile;
        public Sprite Sprite;

        public VisualTile(Point gridPos)
        {
            Tile = Grid.I.GetTile(gridPos);
            if (Tile == null) return;

            switch (Tile.Type)
            {
                case TileType.Platform:
                    Sprite = new Sprite(Loader.Platforms[(int)Tile.PlatformType], Tile.Position);
                    TilePlatform();
                    break;

                case TileType.Terrain:
                    Sprite = new Sprite(Loader.Terrains[(int)Tile.TerrainType], Tile.Position);
                    TileTerrain();
                    break;

                case TileType.Wall:
                    Sprite = new Sprite(Loader.Walls[(int)Tile.WallType], Tile.Position);
                    TileWall();
                    break;
            }
            Sprite.Scale = new Vector2(2, 2);
        }
        public int GetAroundTiles()
        {
            int res = 0;
            Tile[] tiles = new Tile[4];
            tiles[0] = Grid.I.GetTile(new Point(Tile.GridPos.X - 1, Tile.GridPos.Y));
            tiles[1] = Grid.I.GetTile(new Point(Tile.GridPos.X, Tile.GridPos.Y - 1));
            tiles[2] = Grid.I.GetTile(new Point(Tile.GridPos.X + 1, Tile.GridPos.Y));
            tiles[3] = Grid.I.GetTile(new Point(Tile.GridPos.X, Tile.GridPos.Y + 1));

            for (int i = 0; i < 4; i++)
            {
                if (tiles[i] != null)
                {
                    bool sameType = false;
                    if (Tile.Type == tiles[i].Type)
                    {
                        switch (Tile.Type)
                        {
                            case TileType.Platform:
                                sameType = Tile.PlatformType == tiles[i].PlatformType;
                                break;
                            case TileType.Terrain:
                                sameType = Tile.TerrainType == tiles[i].TerrainType;
                                break;
                            case TileType.Wall:
                                sameType = Tile.WallType == tiles[i].WallType;
                                break;
                        }
                    }
                    if (sameType) res += (int)Math.Pow(2, i);
                }
            }

            return res;
        }
        public void TilePlatform(bool recur = true)
        {
            if (Tile == null) return;

            int tilesAround = GetAroundTiles();
            Sprite.SetFromSpriteSheet(PlatformPoints[tilesAround], new Point(3, 1));

            // Change tile around one time
            if (recur)
            {
                VisualTile left = VisualGrid.I.GetTile(new Point(Tile.GridPos.X - 1, Tile.GridPos.Y));
                if (left != null && left.Tile != null && Tile.Type == left.Tile.Type && left.Tile.PlatformType == Tile.PlatformType) left.TilePlatform(false);
            }
        }
        public void TileTerrain(bool recur = true)
        {
            if (Tile == null) return;

            int tilesAround = GetAroundTiles();
            Sprite.SetFromSpriteSheet(TerrainPoints[tilesAround], new Point(5, 3));

            // Change tile around one time
            if (recur)
            {
                VisualTile left = VisualGrid.I.GetTile(new Point(Tile.GridPos.X - 1, Tile.GridPos.Y));
                if (left != null && left.Tile != null && Tile.Type == left.Tile.Type && left.Tile.TerrainType == Tile.TerrainType) left.TileTerrain(false);
                VisualTile up = VisualGrid.I.GetTile(new Point(Tile.GridPos.X, Tile.GridPos.Y - 1));
                if (up != null && up.Tile != null && Tile.Type == up.Tile.Type && up.Tile.TerrainType == Tile.TerrainType) up.TileTerrain(false);
            }
        }
        public void TileWall(bool recur = true)
        {
            if (Tile == null) return;

            int tilesAround = GetAroundTiles();
            Sprite.SetFromSpriteSheet(WallPoints[tilesAround], new Point(4, 3));

            // Change tile around one time
            if (recur)
            {
                VisualTile left = VisualGrid.I.GetTile(new Point(Tile.GridPos.X - 1, Tile.GridPos.Y));
                if (left != null && left.Tile != null && Tile.Type == left.Tile.Type && left.Tile.WallType == Tile.WallType) left.TileWall(false);
                VisualTile up = VisualGrid.I.GetTile(new Point(Tile.GridPos.X, Tile.GridPos.Y - 1));
                if (up != null && up.Tile != null && Tile.Type == up.Tile.Type && up.Tile.WallType == Tile.WallType) up.TileWall(false);
            }
        }
    }
}