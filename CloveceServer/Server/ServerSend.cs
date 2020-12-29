using CloveceServer.Backend;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            using ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteInt((int)ServerPackets.BOARD_STATE);

            IEnumerable<(int id, GameMaster.Tile)> filledTiles = FilterTiles(GameMaster.Instance.Tiles);
            WriteTilesToBuffer(_buffer, filledTiles);

            IEnumerable<(int id, GameMaster.Tile)> startTiles = FilterTiles(GameMaster.Instance.StartTiles);
            WriteTilesToBuffer(_buffer, startTiles);

            IEnumerable<(int id, GameMaster.Tile)> endTiles = FilterTiles(GameMaster.Instance.EndTiles);
            WriteTilesToBuffer(_buffer, endTiles);

            SendDataTo(_sendToPlayer, _buffer.ToArray());
        }

        public static void Rolled(int _sendToPlayer, int rolled)
        {
            using ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteInt((int)ServerPackets.ROLLED);
            _buffer.WriteInt(rolled);
            SendDataTo(_sendToPlayer, _buffer.ToArray());
        }

        public static void GameState(int _sendToPlayer)
        {
            using ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteInt((int)ServerPackets.GAME_STATE);
            _buffer.WriteInt((int)Globals.clients[_sendToPlayer].Player.GameState);
            SendDataTo(_sendToPlayer, _buffer.ToArray());
        }

        private static IEnumerable<(int id, GameMaster.Tile)> FilterTiles(GameMaster.Tile[] allTiles) => allTiles
                .Select((tile, i) => (i, tile))
                .Where(pair => pair.tile.PlayerId.HasValue);

        private static void WriteTilesToBuffer(ByteBuffer _buffer, IEnumerable<(int index, GameMaster.Tile tile)> array)
        {
            _buffer.WriteInt(array.Count());
            foreach (var (index, tile) in array)
            {
                _buffer.WriteInt(index);
                _buffer.WriteInt(tile.PlayerId.Value);
                _buffer.WriteInt(tile.PawnId);
            }
        }
    }
}
