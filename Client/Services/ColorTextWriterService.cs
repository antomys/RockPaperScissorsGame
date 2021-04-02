using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Services
{
    public static class ColorTextWriterService
    {
        public static void PrintLineMessageWithSpecialColor(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        
        public static void PrintMessageWithSpecialColor(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ResetColor();
        }
    }
}
