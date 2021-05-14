﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Models;
using Client.Services;
using Client.Services.RequestProcessor;

namespace Client.Menus
{
    public class StartMenu
    {
        private readonly IAccountMenu _accountMenu;

        private string SessionId { get; set; }

        public StartMenu(IRequestPerformer performer)
        {
            _accountMenu = new AccountMenu(performer);
        }
        
        public async Task<int> StartAsync()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Greeting();
            TextWrite.Print("\n\nPress any key to show start up menu list!", ConsoleColor.Green);
            Console.ReadKey();
            Console.Clear();
            //todo: trying to connect to the server
            await Menu(token);
            return 1;
        }
        private async Task Menu(CancellationToken token)
        {
            while (true)
            {
                TextWrite.Print("" +
                "Please auth to proceed:\n" +
                "1.\tSign up\n" +
                "2.\tLog in\n" +
                "3.\tSee Leaderboard\n" +       //This part will be available after we figure out the statistics
                "4.\tExit", ConsoleColor.DarkYellow);

                TextWrite.Print("\nPlease select an item from the list", ConsoleColor.Green);

                Console.Write("Select -> ");
                if (token.IsCancellationRequested)
                {
                    return;
                }
                var passed = int.TryParse(Console.ReadLine(), out var startMenuInput);
                if (!passed)
                {
                    TextWrite.Print("Unsupported input", ConsoleColor.Red);
                    continue;
                }
                switch (startMenuInput)
                {
                    case 1:
                        await _accountMenu.Register();
                        TextWrite.Print(
                            "\n\nPress any key to back to the start up menu list!", ConsoleColor.Cyan);
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        Account inputAccount;
                        (SessionId, inputAccount) = await _accountMenu.LogIn();
                        if (string.IsNullOrEmpty(SessionId))
                        {
                            Console.WriteLine("Failed to log in");
                            Console.Clear();
                        }
                        else
                        {
                            Console.ReadKey();
                            Console.Clear();
                            await new MainMenu(inputAccount).PlayerMenu();
                        }
                        break;
                    case 3:
                        /*var statistics = await OverallStatistics();
                        if(statistics == null)
                            Console.WriteLine("No statistics so far");
                        else
                        {
                            PrintStatistics(statistics); 
                        }*/
                        break;
                    case 4:
                        if (await _accountMenu.Logout(SessionId))
                        {
                            Console.WriteLine("DEBUG: Logged out");
                            return;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    default:
                        TextWrite.Print("Unsupported input", ConsoleColor.Red);
                        continue;
                }

            }

        }
        
        private static void Greeting()
        {
            TextWrite.Print(
                "Welcome to the world best game ----> Rock-Paper-Scissors!\n" +
                "You are given the opportunity to compete with other users in this wonderful game,\n" +
                "or if you don’t have anyone to play, don’t worry,\n" +
                "you can find a random player or just try your skill with a bot.", ConsoleColor.White);
            TextWrite.Print("(c)Ihor Volokhovych & Michael Terekhov", ConsoleColor.Cyan);
        }
    }
}