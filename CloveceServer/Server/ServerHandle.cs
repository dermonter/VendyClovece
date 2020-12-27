using CloveceServer.Backend;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloveceServer.Server
{
    public class ServerHandle
    {
        private delegate void Packet(int _playerId, byte[] _data);
        private static Dictionary<int, Packet> packets;

        public static void InitPackets()
        {
            Logger.Log(LogType.info1, "Initializing packets..");
            packets = new Dictionary<int, Packet>()
            {
                { (int)ClientPackets.WELCOME_RECEIVED, WelcomeReceived }
            };
        }

        public static void HandleData(int _playerId, byte[] _data)
        {
            byte[] _tempBuffer = (byte[])_data.Clone();
            int _packetLength = 0;

            if (Globals.clients[_playerId].Buffer == null)
            {
                Globals.clients[_playerId].Buffer = new ByteBuffer();
            }

            Globals.clients[_playerId].Buffer.WriteBytes(_data);
            if (Globals.clients[_playerId].Buffer.Count() == 0)
            {
                Globals.clients[_playerId].Buffer.Clear();
                return;
            }

            if (Globals.clients[_playerId].Buffer.Length() >= 4)
            {
                _packetLength = Globals.clients[_playerId].Buffer.ReadInt(false);
                if (_packetLength <= 0)
                {
                    Globals.clients[_playerId].Buffer.Clear();
                    return;
                }
            }

            while (_packetLength > 0 && _packetLength <= Globals.clients[_playerId].Buffer.Length() - 4)
            {
                Globals.clients[_playerId].Buffer.ReadInt();
                _data = Globals.clients[_playerId].Buffer.ReadBytes(_packetLength);
                HandlePackets(_playerId, _data);

                _packetLength = 0;
                if (Globals.clients[_playerId].Buffer.Length() >= 4)
                {
                    _packetLength = Globals.clients[_playerId].Buffer.ReadInt(false);
                    if (_packetLength <= 0)
                    {
                        Globals.clients[_playerId].Buffer.Clear();
                        return;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                Globals.clients[_playerId].Buffer.Clear();
            }
        }

        private static void HandlePackets(int _playerId, byte[] _data)
        {
            using ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteBytes(_data);
            int _packetId = _buffer.ReadInt();

            packets.GetValueOrDefault(_packetId)?.Invoke(_playerId, _data);
        }

        private static void WelcomeReceived(int _playerId, byte[] _data)
        {
            using ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteBytes(_data);
            _buffer.ReadInt();

            string _username = _buffer.ReadString();

            Logger.Log(LogType.info2, "Connection from " + Globals.clients[_playerId].Socket.Client.RemoteEndPoint + " was successful. Username: " + _username);
        }
    }
}
