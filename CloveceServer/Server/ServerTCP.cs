using CloveceServer.Backend;
using System;
using System.Net;
using System.Net.Sockets;

namespace CloveceServer.Server
{
    public class ServerTCP
    {
        private static TcpListener socket;
        private static int port = 42069;

        public static void InitNetwork()
        {
            Logger.Log(LogType.info1, "Starting server on port " + port + "..");
            ServerHandle.InitPackets();
            socket = new TcpListener(IPAddress.Any, port);
            socket.Start();
            socket.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), null);
        }

        private static void ClientConnected(IAsyncResult _result)
        {
            TcpClient _client = socket.EndAcceptTcpClient(_result);
            _client.NoDelay = false;
            socket.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), null);

            Logger.Log(LogType.info1, "Incoming connection from " + _client.Client.RemoteEndPoint.ToString());

            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (Globals.clients[i].Socket == null)
                {
                    Globals.clients[i].Socket = _client;
                    Globals.clients[i].playerId = i;
                    Globals.clients[i].StartClient();
                    return;
                }
            }

            Logger.Log(LogType.warning, "Server full");
        }
    }
}
