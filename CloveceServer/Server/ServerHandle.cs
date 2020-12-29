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
                { (int) ClientPackets.WELCOME_RECEIVED, WelcomeReceived },
                { (int) ClientPackets.ROLL, Roll },
                { (int) ClientPackets.GET_BOARD, GetBoard },
                { (int) ClientPackets.SELECT_PAWN, SelectPawn },
                { (int) ClientPackets.GET_GAMESTATE, GetGameState }
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

        private static void Roll(int _playerId, byte[] _data)
        {
            using ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteBytes(_data);
            _buffer.ReadInt();

            Logger.Log(LogType.info2, Globals.clients[_playerId].Socket.Client.RemoteEndPoint + " attemped roll");
            // roll logic -> send packet to user back
            // save rolled value
            int rolled = Globals.clients[_playerId].Player.Roll();

            ServerSend.Rolled(_playerId, rolled);
        }

        private static void SelectPawn(int _playerId, byte[] _data)
        {
            using ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteBytes(_data);
            _buffer.ReadInt();

            int pawnId = _buffer.ReadInt();

            Logger.Log(LogType.info2, Globals.clients[_playerId].Socket.Client.RemoteEndPoint + " attemped to select a pawn");
            // try selecting the pawn
            // update the board and send back a response
            Globals.clients[_playerId].Player.Move(pawnId);
            ServerSend.PawnMoved(_playerId, pawnId);
        }

        private static void GetBoard(int _playerId, byte[] _data)
        {
            using ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteBytes(_data);
            _buffer.ReadInt();

            Logger.Log(LogType.info2, "Received request to send boad state to " + Globals.clients[_playerId].Socket.Client.RemoteEndPoint);
            // send board state back
            ServerSend.BoardState(_playerId);
        }

        private static void GetGameState(int _playerId, byte[] _data)
        {
            using ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteBytes(_data);
            _buffer.ReadInt();

            Logger.Log(LogType.info2, "Received request to send game state to " + Globals.clients[_playerId].Socket.Client.RemoteEndPoint);
            // send game state back
            ServerSend.GameState(_playerId);
        }
    }
}
