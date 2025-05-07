namespace SeaLegs.Data
{
    public class GameSettings
    {
        /* Design width and height is the target dimensions the game is designed around.
         * These dimensions should fit the aspect ratio set for the SLDisplay component in your razor page.
         * SeaLegs uses these dimensions to calculate the horizontal and vertical scale, to ensure the game
              looks and plays the same regardless of canvas size. */
        public static int DesignWidth { get; private set; } = 1920;
        public static int DesignHeight { get; private set; } = 1080;

        public static void UpdateDesignDimensions(int width, int height)
        {
            DesignWidth = width;
            DesignHeight = height;
            Console.WriteLine($"Design dimensions set to {DesignWidth}, {DesignHeight}");
        }

    }
}
