using System;

namespace Client.Services;

public static class TextWrite
{
    public static void Print(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}