using Bindings;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    public class PlayerState
    {
        public bool Up = false;
        public bool Down = false;
        public bool Left = false;
        public bool Right = false;

        public PlayerState()
        {
            Up = InputSystem.GetKey(Keys.Z);
            Down = InputSystem.GetKey(Keys.S);
            Left = InputSystem.GetKey(Keys.Q);
            Right = InputSystem.GetKey(Keys.D);
        }

        public bool SameAs(PlayerState _other)
        {
            return _other.Up != Up || _other.Down != Down || _other.Left != Left || _other.Right != Right;
        }
    }
}