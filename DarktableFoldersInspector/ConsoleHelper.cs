using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarktableFoldersInspector
{
    public static class ConsoleHelper
    {
        public static void WriteColoredMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        public static void WriteWarning(string message)
        {
            WriteColoredMessage(message, ConsoleColor.Yellow);
        }

        public static void WriteError(string message)
        {
            WriteColoredMessage(message, ConsoleColor.Red);
        }
    }
}
