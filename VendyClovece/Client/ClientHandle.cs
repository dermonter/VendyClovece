using CloveceServer.Backend;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendyClovece.Client
{
    public class ClientHandle
    {
        public delegate void Packet(byte[] _data);
        public static Dictionary<int, Packet> packets;
        private static ByteBuffer buffer;

        public static void InitPackets()
        {
            packets = new Dictionary<int, Packet>()
            {
                {(int) ServerPackets.WELCOME, Welcome }
            };
        }

        public static void HandleData(byte[] _data)
        {
            byte[] _tempBuffer = (byte[])_data.Clone();
            int _packetLength = 0;

            if (buffer == null)
            {
                buffer = new ByteBuffer();
            }

            buffer.WriteBytes(_data);
            if (buffer.Count() == 0)
            {
                buffer.Clear();
                return;
            }

            if (buffer.Length() >= 4)
            {
                _packetLength = buffer.ReadInt(false);
                if (_packetLength <= 0)
                {
                    buffer.Clear();
                    return;
                }
            }

            while (_packetLength > 0 && _packetLength <= buffer.Length() - 4)
            {
                buffer.ReadInt();
                _data = buffer.ReadBytes(_packetLength);
                HandlePackets(_data);

                _packetLength = 0;
                if (buffer.Length() >= 4)
                {
                    _packetLength = buffer.ReadInt(false);
                    if (_packetLength <= 0)
                    {
                        buffer.Clear();
                        return;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                buffer.Clear();
            }
        }

        private static void HandlePackets(byte[] _data)
        {
            using ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteBytes(_data);
            int _packetId = _buffer.ReadInt();

            packets.GetValueOrDefault(_packetId)?.Invoke(_data);
        }

        private static void Welcome(byte[] _data)
        {
            ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteBytes(_data);
            _buffer.ReadInt();
            string _msg = _buffer.ReadString();
            int _myPlayerID = _buffer.ReadInt();
            _buffer.Dispose();
            Console.WriteLine("Message from server: " + _msg);
            ClientTcp.myPlayerId = _myPlayerID;
            ClientSend.WelcomeReceived();
        }
    }
}
