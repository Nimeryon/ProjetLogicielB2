using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    public static class Globals
    {
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
        private static float _DeltaTime;
        public static float DeltaTime10;
    }
}
