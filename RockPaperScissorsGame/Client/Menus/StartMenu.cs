using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Menus.Interfaces;
using Client.Models.Interfaces;
using Client.Services;

namespace Client.Menus
{
    internal class StartMenu : IStartMenu
    {
        private readonly ILoginMenu _loginMenu;
        private readonly IMainMenu _mainMenu;
        private readonly IStatisticsMenu _statisticsMenu;
        private readonly IRegisterMenu _registerMenu;
        private readonly IAccount _playerAccount;

        public StartMenu(
            ILoginMenu loginMenu,
            IMainMenu mainMenu,
            IStatisticsMenu statisticsMenu,
            IRegisterMenu registerMenu,
            IAccount playerAccount)
        {
            _loginMenu = loginMenu;
            _mainMenu = mainMenu;
            _statisticsMenu = statisticsMenu;
            _registerMenu = registerMenu;
            _playerAccount = playerAccount;
        }
        public async Task Start(CancellationToken token)
        {
            while (true)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor("" +
                "Please auth to proceed:\n" +
                "1.\tSign up\n" +
                "2.\tLog in\n" +
                "3.\tSee Leaderboard\n" +       //This part will be available after we figure out the statistics
                "4.\tExit", ConsoleColor.DarkYellow);

                ColorTextWriterService.PrintLineMessageWithSpecialColor("\nPlease select an item from the list", ConsoleColor.Green);

                Console.Write("Select -> ");
                if (token.IsCancellationRequested)
                {
                    return;
                }
                var passed = int.TryParse(Console.ReadLine(), out int startMenuInput);
                if (!passed)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Unsupported input", ConsoleColor.Red);
                    continue;
                }
                switch (startMenuInput)
                {
                    case 1:
                        await _registerMenu.Registration();
                        ColorTextWriterService.PrintLineMessageWithSpecialColor(
                            "\n\nPress any key to back to the start up menu list!", ConsoleColor.Cyan);
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        var status = await _loginMenu.LogIn();
                        if (status == 0)
                        {
                            await _mainMenu.Start();
                        }
                        else
                        {
                            ColorTextWriterService.PrintLineMessageWithSpecialColor(
                                "\n\nPress any key to back to the start up menu list!", ConsoleColor.Cyan);
                            Console.ReadKey();
                            Console.Clear();
                        }
                        break;
                    case 3:
                        await _statisticsMenu.OverallStatistics();
                        break;
                    case 4:
                        if (_playerAccount.Login != null)
                            await _loginMenu.Logout();
                        return;
                    default:
                        ColorTextWriterService.PrintLineMessageWithSpecialColor("Unsupported input", ConsoleColor.Red);
                        continue;
                }
            }
        }
    }
}