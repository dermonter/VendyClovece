namespace VendyClovece.Online
{
    public enum ServerPackets
    {
        PLAYER_REGISTERED = 0,
        BOARD_STATE = 1,
        ROLLED = 2,
        GAME_STATE = 3
    }

    public enum ClientPackets
    {
        REGISTER_PLAYER = 0,
        ROLL = 1,
        SELECT_PAWN = 2,
        GET_BOARD = 3,
        GET_GAMESTATE = 4
    }
}
