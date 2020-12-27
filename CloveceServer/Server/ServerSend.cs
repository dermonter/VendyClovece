using CloveceServer.Backend;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloveceServer.Server
{
    public class ServerSend
    {
        public static void SendDataTo(int _playerId, byte[] _data)
        {
            try
            {
                if (Globals.clients[_playerId].Socket == null)
                    return;

                using ByteBuffer _buffer = new ByteBuffer();
                _buffer.WriteInt(_data.GetUpperBound(0) - _data.GetLowerBound(0) + 1);
                _buffer.WriteBytes(_data);
                Globals.clients[_playerId].Stream.BeginWrite(_buffer.ToArray(), 0, _buffer.ToArray().Length, null, null);
            }
            catch (Exception _ex)
            {
                Logger.Log(LogType.error, "Error sending data to player " + _playerId + ": " + _ex);
            }
        }

        public static void Welcome(int _sendToPlayer, string _msg)
        {
            using ByteBuffer _buffer = new ByteBuffer();

            _buffer.WriteInt((int)ServerPackets.WELCOME);
            _buffer.WriteString(_msg);
            _buffer.WriteInt(_sendToPlayer);
            SendDataTo(_sendToPlayer, _buffer.ToArray());
        }

        public static void PawnMoved(int _sendToPlayer, int pawnId)
        {

        }

        public static void BoardState(int _sendToPlayer)
        {

        }

        public static void Rolled(int _sendToPlayer)
        {
            using ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteInt((int)ServerPackets.ROLLED);
            var generator = new Random();
            int rolled = generator.Next(1, 7);
            _buffer.WriteInt(rolled);
            SendDataTo(_sendToPlayer, _buffer.ToArray());
        }

        public static void GameState(int _sendToPlayer)
        {

        }
    }
}
