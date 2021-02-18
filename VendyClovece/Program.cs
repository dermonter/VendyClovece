using System;
using VendyClovece.Backend;
using VendyClovece.Online;

namespace VendyClovece
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            ClientTcp.Init();

            using var game = new GameLogic();
            game.Run();
        }
    }
}
