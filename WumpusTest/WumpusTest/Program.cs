using System;

namespace WumpusTest
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            //using (Game1WithoutKinect game = new Game1WithoutKinect())
            {
                game.Run();
            }
        }
    }
#endif
}

