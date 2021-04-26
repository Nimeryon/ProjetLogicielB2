using System;
using System.Collections.Generic;
using System.Text;

namespace Bindings
{
    public static class Helpers
    {
        public static Random Rnd = new Random();

        public static float RandomRange(float min, float max)
        {
            return (float)Rnd.NextDouble() * (max - min + 1) + min;
        }
        public static int RandomRange(int min, int max)
        {
            return Rnd.Next(min, max);
        }
    }
}
