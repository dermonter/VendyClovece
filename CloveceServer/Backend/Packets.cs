namespace CloveceServer.Backend
{
    public enum ServerPackets
    {
        WELCOME = 0,
        PAWN_MOVED = 1,
        BOARD_STATE = 2,
        ROLLED = 3,
        GAME_STATE = 4
    }

    public enum ClientPackets
    {
        WELCOME_RECEIVED = 0,
        ROLL = 1,
        SELECT_PAWN = 2,
        GET_BOARD = 3,
        GET_GAMESTATE = 4
    }
}
