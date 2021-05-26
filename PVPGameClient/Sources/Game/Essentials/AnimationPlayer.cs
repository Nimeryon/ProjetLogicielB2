using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    public class AnimationPlayer : IDisposable
    {
        public Sprite Sprite;
        public Animation Animation;
        public int FrameIndex = 0;
        public float Time = 0.0f;

        public AnimationPlayer(Sprite sprite, Animation animation)
        {
            Sprite = sprite;
            PlayAnimation(animation);

            GameHandler.OnUpdate += Update;
        }

        public void PlayAnimation(Animation animation)
        {
            // If this animation is already running, do not restart it.
            if (Animation == animation) return;

            // Start the new animation.
            Animation = animation;
            FrameIndex = 0;
            Time = 0.0f;

            // Set the sprite
            Sprite.SetTexture(animation.Texture);
            Sprite.SetFromSpriteSheet(new Point(FrameIndex, 0), new Point(Animation.FrameCount, 1));
        }

        public void Update()
        {
            if (Animation == null) return;

            Time += Globals.DeltaTime;
            while (Time > Animation.Duration)
            {
                Time -= Animation.Duration;

                if (Animation.IsLooping) FrameIndex = (FrameIndex + 1) % Animation.FrameCount;
                else FrameIndex = Math.Min(FrameIndex + 1, Animation.FrameCount - 1);

                Sprite.SetFromSpriteSheet(new Point(FrameIndex, 0), new Point(Animation.FrameCount, 1));
            }
        }

        public void Dispose()
        {
            GameHandler.OnUpdate -= Update;
        }
    }
}
