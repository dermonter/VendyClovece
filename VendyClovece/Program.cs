using System;
using VendyClovece.Online;

namespace VendyClovece
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            ClientTcp.Init();
            ClientSend.RegisterPlayer();

            using var game = new GameLogic();
            game.Run();
        }
    }
}
