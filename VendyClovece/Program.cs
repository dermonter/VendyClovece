using System;
using VendyClovece.Client;

namespace VendyClovece
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            ClientTcp.ConnectToServer();

            using var game = new GameLogic();
            game.Run();
        }
    }
}
