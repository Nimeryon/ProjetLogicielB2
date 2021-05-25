using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace PVPGameClient
{
    public static class Loader
    {
        public static ContentManager Content;
        // Textures
        public static Texture2D[] BackGrounds;
        public static Texture2D[] Platforms;
        public static Texture2D[] Terrains;
        public static Texture2D[] Walls;
        public static Texture2D Debug;

        public static void Load()
        {
            LoadBackGrounds();
            LoadPlatforms();
            LoadTerrains();
            LoadWalls();
            Debug = LoadTexture("Sprites/Debug");
        }
        private static void LoadBackGrounds()
        {
            string[] names = Enum.GetNames(typeof(BackGroundColor));
            BackGrounds = new Texture2D[names.Length];

            for(int i = 0; i < names.Length; i++)
            {
                BackGrounds[i] = LoadTexture(string.Format("Sprites/BackGrounds/{0}", names[i]));
            }
        }
        private static void LoadPlatforms()
        {
            Platforms = new Texture2D[3];

            for (int i = 1; i <= Platforms.Length; i++)
            {
                Platforms[i - 1] = LoadTexture(string.Format("Sprites/Terrains/platform_{0}", i));
            }
        }
        private static void LoadTerrains()
        {
            Terrains = new Texture2D[7];

            for (int i = 1; i <= Terrains.Length; i++)
            {
                Terrains[i - 1] = LoadTexture(string.Format("Sprites/Terrains/terrain_{0}", i));
            }
        }
        private static void LoadWalls()
        {
            Walls = new Texture2D[4];

            for (int i = 1; i <= Walls.Length; i++)
            {
                Walls[i - 1] = LoadTexture(string.Format("Sprites/Terrains/wall_{0}", i));
            }
        }
        public static Texture2D LoadTexture(string _name)
        {
            return Content.Load<Texture2D>(_name);
        }
    }
}