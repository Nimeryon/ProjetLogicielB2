using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    public class InputSystem : IDisposable
    {
        private static KeyboardState _oldState = new KeyboardState();
        private static KeyboardState _currentState = new KeyboardState();

        public InputSystem()
        {
            GameHandler.OnBeforeUpdate += Update;
        }

        public static bool GetKey(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }
        public static bool GetKeyDown(Keys key)
        {
            return _currentState.IsKeyDown(key) && _oldState.IsKeyUp(key);
        }
        public static bool GetKeyUp(Keys key)
        {
            return _currentState.IsKeyUp(key) && _oldState.IsKeyDown(key);
        }
        public void Dispose()
        {
            GameHandler.OnBeforeUpdate -= Update;
        }

        private void Update()
        {
            _oldState = _currentState;
            _currentState = Keyboard.GetState();
        }
    }
}
