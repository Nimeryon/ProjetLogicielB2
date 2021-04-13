using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    class FPSCounter : IDisposable
    {
        private Text Counter;

        public FPSCounter()
        {
            Counter = new Text(GameHandler._font, "0", Vector2.Zero);
            TickSystem.OnTickEvent += Update;
        }

        public void HideShow()
        {
            Counter.Visible = !Counter.Visible;
        }
        public void Dispose()
        {
            Counter.Dispose();
            TickSystem.OnTickEvent -= Update;
        }
        private void Update()
        {
            Counter.SetText(Math.Ceiling(1  / Globals.DeltaTime).ToString());
        }
    }
}
