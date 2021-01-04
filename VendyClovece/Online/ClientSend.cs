using System;
using System.Collections.Generic;
using System.Text;

namespace VendyClovece.Online
{
    public class ClientSend
    {
        public static void RegisterPlayer()
        {
            using ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ClientPackets.REGISTER_PLAYER);
            ClientTcp.SendPacket(buffer.ToArray());
        }

        public static void Roll()
        {
            using ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ClientPackets.ROLL);
            buffer.WriteInt(ClientTcp.myPlayerId);
            ClientTcp.SendPacket(buffer.ToArray());
        }

        public static void SelectPawn(int pawnId)
        {
            using ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ClientPackets.GET_BOARD);
            buffer.WriteInt(ClientTcp.myPlayerId);
            buffer.WriteInt(pawnId);
            ClientTcp.SendPacket(buffer.ToArray());
        }

        public static void GetBoard()
        {
            using ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ClientPackets.GET_BOARD);
            ClientTcp.SendPacket(buffer.ToArray());
        }

        public static void GetGameState()
        {
            using ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ClientPackets.GET_GAMESTATE);
            buffer.WriteInt(ClientTcp.myPlayerId);
            ClientTcp.SendPacket(buffer.ToArray());
        }
    }
}
