using System;

namespace PVPGameClient
{
    public class TickSystem : IDisposable
    {
        public delegate void TickEvent();
        public static event TickEvent OnMiniTickEvent;
        public static event TickEvent OnSmallTickEvent;
        public static event TickEvent OnLittleTickEvent;
        public static event TickEvent OnTickEvent;
        public static event TickEvent OnLongTickEvent;

        private const float TICK_TIMER_MAX = .1f;

        private static int tick;
        private float tickTimer;

        // Getters / Setters
        public static int GetTick()
        {
            return tick;
        }
        public static void SetTick(int _tick)
        {
            tick = _tick;
        }

        // Constructors
        public TickSystem()
        {
            GameHandler.OnBeforeUpdate += Update;
        }

        private void Update()
        {
            tickTimer += Globals.DeltaTime;
            if (tickTimer >= TICK_TIMER_MAX)
            {
                tickTimer -= TICK_TIMER_MAX;
                tick++;
                OnMiniTickEvent?.Invoke();

                if (tick % 2 == 0)
                {
                    OnSmallTickEvent?.Invoke();
                }

                if (tick % 5 == 0)
                {
                    OnLittleTickEvent?.Invoke();
                }

                if (tick % 10 == 0)
                {
                    OnTickEvent?.Invoke();
                }

                if (tick % 50 == 0)
                {
                    OnLongTickEvent?.Invoke();
                }
            }
        }

        public void Dispose()
        {
            GameHandler.OnBeforeUpdate -= Update;
        }
    }
}
