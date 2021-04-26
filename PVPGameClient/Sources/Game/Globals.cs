using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    public static class Globals
    {
        public static GameTime GameTime
        {
            get
            {
                return _GameTime;
            }
            set
            {
                _GameTime = value;
                DeltaTime = (float)_GameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        public static float DeltaTime
        {
            get
            {
                return _DeltaTime;
            }
            set
            {
                _DeltaTime = value;
                DeltaTime10 = value * 10;
            }
        }
        public static float DeltaTime10;

        private static GameTime _GameTime;
        private static float _DeltaTime;
    }
}
