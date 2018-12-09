using System;

namespace Super_Platformer
{

    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (SuperPlatformerGame game = new SuperPlatformerGame())
                game.Run();
        }
    }

}
