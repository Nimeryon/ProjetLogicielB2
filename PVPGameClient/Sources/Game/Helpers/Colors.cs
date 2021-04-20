using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PVPGameClient
{
    public static class Colors
    {
        public static void ReplaceColor(Texture2D texture, Dictionary<Color, Color> fromTo)
        {
            Color[] colors = new Color[texture.Width * texture.Height];
            texture.GetData(colors);

            for (int i = 0; i < colors.Length; i ++)
            {
                if (fromTo.TryGetValue(colors[i], out Color color)) colors[i] = color;
            }

            texture.SetData(colors);
        }
        public static Color DarkColor(Color color, int darker)
        {
            return new Color(
                Math.Clamp(color.R - darker, 0, 255),
                Math.Clamp(color.G - darker, 0, 255),
                Math.Clamp(color.B - darker, 0, 255)
            );
        }
    }
}