using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameLibrary
{
    public class Inputs
    {
        public bool Left;
        public bool Right;
        public bool Jump;
        public bool Attack;

        public Inputs()
        {
            Left = false;
            Right = false;
            Jump = false;
            Attack = false;
        }
        public Inputs(bool _left = false, bool _right = false, bool _jump = false, bool _attack = false)
        {
            Left = _left;
            Right = _right;
            Jump = _jump;
            Attack = _attack;
        }

        public bool SameAs(Inputs _other)
        {
            return _other.Left != Left ||
                   _other.Right != Right ||
                   _other.Jump != Jump ||
                   _other.Attack != Attack;
        }
    }
}