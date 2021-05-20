using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    public static class Loader
    {
        public static ContentManager Content;
        public static Texture2D[] BackGrounds;

        public static void Load()
        {
            LoadBackGrounds();
        }
        public static void LoadBackGrounds()
        {
            string[] names = Enum.GetNames(typeof(BackGroundColor));
            BackGrounds = new Texture2D[names.Length];

            for(int i = 0; i < names.Length; i++)
            {
                BackGrounds[i] = LoadTexture(string.Format("Sprites/BackGrounds/{0}", names[i]));
            }
        }
        public static Texture2D LoadTexture(string _name)
        {
            return Content.Load<Texture2D>(_name);
        }
    }
}
