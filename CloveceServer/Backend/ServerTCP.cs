using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CloveceServer.Backend
{
    public class ServerTCP
    {
        private static TcpListener socket;
        private static int port = 42069;

        public static void InitNetwork()
        {
            socket = new TcpListener(IPAddress.Any, port);
            socket.Start();
            socket.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), null);
        }

        private static void ClientConnected(IAsyncResult _result)
        {
            TcpClient _client = socket.EndAcceptTcpClient(_result);
            _client.NoDelay = false;
            socket.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), null);
        }
    }
}
