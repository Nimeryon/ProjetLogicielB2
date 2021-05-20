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

        private static MouseState _oldMouseState = new MouseState();
        private static MouseState _currentMouseState = new MouseState();


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
        public static bool GetMouseKey(int button)
        {
            switch (button)
            {
                case 0:
                    return _currentMouseState.LeftButton == ButtonState.Pressed;

                case 1:
                    return _currentMouseState.MiddleButton == ButtonState.Pressed;

                case 2:
                    return _currentMouseState.RightButton == ButtonState.Pressed;

                default: return false;
            }
        }
        public static bool GetMouseKeyDown(int button)
        {
            switch (button)
            {
                case 0:
                    return _currentMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released;

                case 1:
                    return _currentMouseState.MiddleButton == ButtonState.Pressed && _oldMouseState.MiddleButton == ButtonState.Released;

                case 2:
                    return _currentMouseState.RightButton == ButtonState.Pressed && _oldMouseState.RightButton == ButtonState.Released;

                default: return false;
            }
        }
        public static bool GetMouseKeyUp(int button)
        {
            switch (button)
            {
                case 0:
                    return _currentMouseState.LeftButton == ButtonState.Released && _oldMouseState.LeftButton == ButtonState.Pressed;

                case 1:
                    return _currentMouseState.MiddleButton == ButtonState.Released && _oldMouseState.MiddleButton == ButtonState.Pressed;

                case 2:
                    return _currentMouseState.RightButton == ButtonState.Released && _oldMouseState.RightButton == ButtonState.Pressed;

                default: return false;
            }
        }
        public void Dispose()
        {
            GameHandler.OnBeforeUpdate -= Update;
        }

        private void Update()
        {
            _oldState = _currentState;
            _currentState = Keyboard.GetState();

            _oldMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }
    }
}
