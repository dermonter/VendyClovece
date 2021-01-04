using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SimpleServer.Backend;

namespace SimpleServer.Server
{
    public class ServerHandle
    {
        private delegate void Packet(TcpClient client, ByteBuffer data);
        private Dictionary<ClientPackets, Packet> packets;

        public ServerHandle()
        {
            packets = new Dictionary<ClientPackets, Packet>()
            {
                { ClientPackets.REGISTER_PLAYER, RegisterPlayer },
                { ClientPackets.GET_BOARD, GetBoard },
                { ClientPackets.GET_GAMESTATE, GetGameState },
            };
        }

        private void GetGameState(TcpClient client, ByteBuffer data)
        {
            int playerId = data.ReadInt();

            NetworkStream stream = client.GetStream();
            stream.Write(ServerSend.SendGameState(playerId));
        }

        private void GetBoard(TcpClient client, ByteBuffer data)
        {
            // send board state back
            NetworkStream stream = client.GetStream();
            stream.Write(ServerSend.BoardState());
        }

        public void HandlePackets(TcpClient client, ByteBuffer data)
        {
            int packetCount = data.ReadInt();

            for (int i = 0; i < packetCount; i++)
            {
                ClientPackets packetId = (ClientPackets)data.ReadInt();
                packets.GetValueOrDefault(packetId)?.Invoke(client, data);
            }
        }

        private void RegisterPlayer(TcpClient client, ByteBuffer data)
        {
            Console.WriteLine("Trying to register a new player");

            int playerId = GameMaster.Instance.RegisterPlayer();
            if (playerId < 0)
                return;

            NetworkStream stream = client.GetStream();
            stream.Write(ServerSend.RegisteredPlayer(playerId));
        }
    }
}
