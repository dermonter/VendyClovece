using SimpleServer.Backend;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SimpleServer.Server
{
    public static class ServerSend
    {
        private static void SendPacket()
        {

        }

        public static byte[] RegisteredPlayer(int playerId)
        {
            using ByteBuffer sendBuffer = new ByteBuffer();
            Console.WriteLine("Found empty player slot at {0}", playerId);
            // number of packets
            sendBuffer.WriteInt(3);

            // registered player packet
            sendBuffer.WriteInt((int)ServerPackets.PLAYER_REGISTERED);
            sendBuffer.WriteInt(playerId);

            // board state
            WriteBoardStateToBuffer(sendBuffer);

            // game state
            WriteGameStateToBuffer(sendBuffer, playerId);
            return sendBuffer.ToArray();
        }

        public static byte[] SendPawnMoved(int playerId)
        {
            using ByteBuffer sendBuffer = new ByteBuffer();
            // number of packets
            sendBuffer.WriteInt(2);

            // board state
            WriteBoardStateToBuffer(sendBuffer);

            // game state
            WriteGameStateToBuffer(sendBuffer, playerId);
            return sendBuffer.ToArray();
        }

        public static byte[] BoardState()
        {
            using ByteBuffer sendBuffer = new ByteBuffer();
            sendBuffer.WriteInt(1);
            sendBuffer.WriteInt((int)ServerPackets.BOARD_STATE);
            WriteBoardStateToBuffer(sendBuffer);
            return sendBuffer.ToArray();
        }

        public static byte[] SendGameState(int playerId)
        {
            using ByteBuffer sendBuffer = new ByteBuffer();
            sendBuffer.WriteInt(1);
            sendBuffer.WriteInt((int)ServerPackets.GAME_STATE);
            WriteGameStateToBuffer(sendBuffer, playerId);
            return sendBuffer.ToArray();
        }

        public static byte[] SendRolled(int rolled)
        {
            using ByteBuffer sendBuffer = new ByteBuffer();
            sendBuffer.WriteInt(1);
            sendBuffer.WriteInt((int)ServerPackets.ROLLED);
            sendBuffer.WriteInt(rolled);
            return sendBuffer.ToArray();
        }

        private static void WriteBoardStateToBuffer(ByteBuffer buffer)
        {
            buffer.WriteInt((int)ServerPackets.BOARD_STATE);

            IEnumerable<(int id, GameMaster.Tile)> filledTiles = FilterTiles(GameMaster.Instance.Tiles);
            WriteTilesToBuffer(buffer, filledTiles);

            IEnumerable<(int id, GameMaster.Tile)> startTiles = FilterTiles(GameMaster.Instance.StartTiles);
            WriteTilesToBuffer(buffer, startTiles);

            IEnumerable<(int id, GameMaster.Tile)> endTiles = FilterTiles(GameMaster.Instance.EndTiles);
            WriteTilesToBuffer(buffer, endTiles);
        }

        private static void WriteTilesToBuffer(ByteBuffer buffer, IEnumerable<(int index, GameMaster.Tile tile)> array)
        {
            buffer.WriteInt(array.Count());
            foreach (var (index, tile) in array)
            {
                buffer.WriteInt(index);
                buffer.WriteInt(tile.PlayerId.Value);
                buffer.WriteInt(tile.PawnId);
            }
        }

        private static void WriteGameStateToBuffer(ByteBuffer buffer, int playerId)
        {
            buffer.WriteInt((int)ServerPackets.GAME_STATE);
            buffer.WriteInt((int)GameMaster.Instance.Players[playerId].GameState);
        }

        private static IEnumerable<(int id, GameMaster.Tile)> FilterTiles(GameMaster.Tile[] allTiles) => allTiles
                .Select((tile, i) => (i, tile))
                .Where(pair => pair.tile.PlayerId.HasValue);
    }
}
