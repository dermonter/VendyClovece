﻿using System;

namespace VendyClovece
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new GameLogic();
            game.Run();
        }
    }
}
