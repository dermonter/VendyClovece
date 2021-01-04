using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace VendyClovece.Online
{
    public class ClientTcp
    {
        private static string ip = "127.0.0.1";
        private static int port = 42069;
        public static int myPlayerId = 0;

        public static string txt;

        private const int BUFFER_SIZE = 1024 * 1024;

        public static void Init()
        {
            ClientHandle.Init();
        }

        public static void SendPacket(byte[] data)
        {
            TcpClient socket = new TcpClient();

            socket.Connect(ip, port);
            NetworkStream stream = socket.GetStream();
            stream.Write(data, 0, data.Length);

            byte[] receiveBuffer = new byte[BUFFER_SIZE];
            int receiveSize;

            using (ByteBuffer buffer = new ByteBuffer())
            {
                receiveSize = stream.Read(receiveBuffer, 0, receiveBuffer.Length);

                byte[] tempBuffer = new byte[receiveSize];
                Array.Copy(receiveBuffer, 0, tempBuffer, 0, receiveSize);
                buffer.WriteBytes(receiveBuffer);

                ClientHandle.HandlePackets(buffer);
            }
            stream.Dispose();
            socket.Close();
        }
    }
}
