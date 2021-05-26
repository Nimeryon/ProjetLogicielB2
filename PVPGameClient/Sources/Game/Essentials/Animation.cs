using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    public class Animation
    {
        public Texture2D Texture;
        public float Duration = 1/20f;
        public int FrameCount;
        public bool IsLooping;

        public Animation(Texture2D texture, int frameCount, bool isLooping)
        {
            Texture = texture;
            FrameCount = frameCount;
            IsLooping = isLooping;
        }
    }
}
