using CloveceServer.Backend;
using CloveceServer.Server;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CloveceServer.Client
{
    public class Client
    {
        public int playerId;
        public bool isPlaying = false;

        public TcpClient Socket { get; set; }
        public NetworkStream Stream { get; private set; }
        public Player Player { get; private set; }
        public ByteBuffer Buffer { get; set; }
        private byte[] receiveBuffer;

        private const int BUFFER_SIZE = 4096;

        public void StartClient()
        {
            Socket.ReceiveBufferSize = BUFFER_SIZE;
            Socket.SendBufferSize = BUFFER_SIZE;

            Stream = Socket.GetStream();
            receiveBuffer = new byte[Socket.ReceiveBufferSize];
            Stream.BeginRead(receiveBuffer, 0, Socket.ReceiveBufferSize, ReceivedData, null);
            GameState gameState = playerId == 0 ? GameState.YOUR_TURN_NOT_ROLLED : GameState.OPONENT_TURN;
            Player = new Player(playerId, gameState);
            ServerSend.Welcome(playerId, "Welcome to the server");
        }

        private void ReceivedData(IAsyncResult _result)
        {
            try
            {
                int _byteLength = Stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    CloseConnection();
                    return;
                }

                byte[] _tempBuffer = new byte[_byteLength];
                Array.Copy(receiveBuffer, _tempBuffer, _byteLength);
                ServerHandle.HandleData(playerId, _tempBuffer);
                Stream.BeginRead(receiveBuffer, 0, Socket.ReceiveBufferSize, ReceivedData, null);
            } catch (Exception _ex)
            {
                Logger.Log(LogType.error, "Error while receiving data: " + _ex);
                CloseConnection();
                return;
            }
        }

        private void CloseConnection()
        {
            Logger.Log(LogType.info1, "Connection from " + Socket.Client.RemoteEndPoint.ToString() + " has been terminated");

            Player = null;
            isPlaying = false;
            Socket.Close();
            Socket = null;
        }
    }
}
