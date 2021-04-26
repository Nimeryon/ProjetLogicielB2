using System;
using System.Collections.Generic;
using System.Text;

namespace Bindings
{
    public enum ServerPackets
    {
        // Serveur response 1X
        ServerConnected = 10,

        // Serveur state 2X
        ServerPlayersState = 21,

        // Serveur navigation 3X
        ServerPlayerConnect = 30,
        ServerPlayerDisconnect = 31
    }

    public enum ClientPackets
    {
        // Client response 1X 
        ClientLogin = 10,

        // Client state 2X 
        ClientInputs = 20

        // Client navigation 3X
    }
}