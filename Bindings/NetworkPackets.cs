using System;
using System.Collections.Generic;
using System.Text;

namespace Bindings
{
    public enum ServerPackets
    {
        ServerOK = 1,
        ServerConnected = 2,
    }

    public enum ClientPackets
    {
        ClientLogin = 1,
        ClientMovement = 2,
    }
}
