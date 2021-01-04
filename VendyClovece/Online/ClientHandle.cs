using System.Collections.Generic;
using VendyClovece.Backend;

namespace VendyClovece.Online
{
    public class ClientHandle
    {
        private delegate void Packet(ByteBuffer data);
        private static Dictionary<ServerPackets, Packet> packets;

        public static void Init()
        {
            packets = new Dictionary<ServerPackets, Packet>()
            {
                { ServerPackets.PLAYER_REGISTERED, RegisteredPlayer },
                { ServerPackets.BOARD_STATE, BoardState },
                { ServerPackets.GAME_STATE, ReceivedGameState },
                { ServerPackets.ROLLED, Rolled }
            };
        }

        private static void Rolled(ByteBuffer data)
        {
            int rolled = data.ReadInt();

            GameMaster.Instance.roll = rolled;
            GameMaster.Instance.gameState = GameState.YOUR_TURN_ROLLED;
        }

        private static void ReceivedGameState(ByteBuffer data)
        {
            GameState gameState = (GameState)data.ReadInt();
            GameMaster.Instance.gameState = gameState;
        }

        private static void BoardState(ByteBuffer data)
        {
            ReadTiles(data, GameMaster.Instance.Board.Tiles);
            ReadTiles(data, GameMaster.Instance.Board.StartTiles);
            ReadTiles(data, GameMaster.Instance.Board.EndTiles);
        }

        private static void RegisteredPlayer(ByteBuffer data)
        {
            int playerId = data.ReadInt();
            ClientTcp.myPlayerId = playerId;
        }

        public static void HandlePackets(ByteBuffer data)
        {
            int packetCount = data.ReadInt();

            for (int i = 0; i < packetCount; i++)
            {
                ServerPackets packetId = (ServerPackets)data.ReadInt();
                packets.GetValueOrDefault(packetId)?.Invoke(data);
            }
        }

        private static void ReadTiles(ByteBuffer _buffer, Tile[] tiles)
        {
            int count = _buffer.ReadInt();
            for (int i = 0; i < count; i++)
            {
                // set pawn to that tile
                // tile index, playerId, pawnId
                int tileIndex = _buffer.ReadInt();
                int playerId = _buffer.ReadInt();
                int pawnId = _buffer.ReadInt();

                GameMaster.Instance.Players[playerId][pawnId].CurrentTile = tiles[tileIndex];
            }
        }
    }
}
