using CloveceServer.Backend;
using System;

namespace VendyClovece.Client
{
    public class ClientSend
    {
        public static void SendDataToServer(byte[] _data)
        {
            try
            {
                if (ClientTcp.Socket == null)
                    return;

                using ByteBuffer _buffer = new ByteBuffer();
                _buffer.WriteInt(_data.GetUpperBound(0) - _data.GetLowerBound(0) + 1);
                _buffer.WriteBytes(_data);
                ClientTcp.Stream.BeginWrite(_buffer.ToArray(), 0, _buffer.ToArray().Length, null, null);
            }
            catch (Exception _ex)
            {
                Console.WriteLine("Error sending data to server: " + _ex);
            }
        }

        public static void WelcomeReceived()
        {
            using ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteInt((int)ClientPackets.WELCOME_RECEIVED);
            _buffer.WriteString("Test player name");
            SendDataToServer(_buffer.ToArray());
        }
    }
}
