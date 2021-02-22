﻿using System;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            try
            {
                var emulator = new ClientAppEmulator();
                return await emulator.StartAsync();
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}