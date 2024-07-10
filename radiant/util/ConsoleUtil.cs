// Radiant Console Utility

using System;

namespace radiant.util
{
    public class ConsoleUtil
    {
        public enum MessageType
        {
            INFO, WARN, ERR, SUCCESS
        };
        public static ConsoleColor[] colors = new ConsoleColor[]
        {
            ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.Green
        };

        public static void Message(MessageType type, string message)
        {
            ConsoleColor cachedbg = Console.BackgroundColor;
            ConsoleColor cachedfg = Console.ForegroundColor;

            // This part is not efficient but Cosmos doesn't support enum conversion to int or string :/ 
            int idx = type switch
            {
                MessageType.INFO => 0,
                MessageType.WARN => 1,
                MessageType.ERR => 2,
                MessageType.SUCCESS => 3,
                _ => 0
            };
            string name = type switch
            {
                MessageType.INFO => "INFO",
                MessageType.WARN => "WARN",
                MessageType.ERR => "ERR",
                MessageType.SUCCESS => "SUCCESS",
                _ => "INFO"
            };
            int freq = type switch
            {
                MessageType.INFO => 600,
                MessageType.WARN => 500,
                MessageType.ERR => 500,
                MessageType.SUCCESS => 800,
                _ => 600
            };

            Console.BackgroundColor = colors[idx];
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"[{name}] -> {message}");
            // This is just to remove the warning for the Visual Studio warning
#pragma warning disable CA1416 // Validate platform compatibility
            Console.Beep(freq, 20);
#pragma warning restore CA1416 // Validate platform compatibility

            Console.BackgroundColor = cachedbg;
            Console.ForegroundColor = cachedfg;
        }
    }
}
