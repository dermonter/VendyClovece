using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using VendyClovece.Backend;

namespace VendyClovece.Client
{
    public class ClientTcp
    {
        private static string ip = "127.0.0.1";
        private static int port = 42069;
        public static int myPlayerId = 0;

        public static TcpClient Socket { get; private set; }
        public static NetworkStream Stream { get; private set; }
        private static byte[] receiveBuffer;

        private const int BUFFER_SIZE = 4096;

        public static void ConnectToServer()
        {
            ClientHandle.InitPackets();
            Socket = new TcpClient
            {
                ReceiveBufferSize = BUFFER_SIZE,
                SendBufferSize = BUFFER_SIZE,
                NoDelay = false
            };

            receiveBuffer = new byte[BUFFER_SIZE];
            Socket.BeginConnect(ip, port, ConnectionCallback, Socket);
        }

        private static void ConnectionCallback(IAsyncResult _result)
        {
            Socket.EndConnect(_result);
            
            if (!Socket.Connected)
                return;

            Socket.NoDelay = true;
            Stream = Socket.GetStream();
            Stream.BeginRead(receiveBuffer, 0, BUFFER_SIZE, ReceivedData, null);
        }

        private static void ReceivedData(IAsyncResult _result)
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
                ClientHandle.HandleData(_tempBuffer);
                Stream.BeginRead(receiveBuffer, 0, BUFFER_SIZE, ReceivedData, null);
            }
            catch (Exception _ex)
            {
                CloseConnection();
                return;
            }
        }

        private static void CloseConnection()
        {
            Socket.Close();
        }
    }
}
