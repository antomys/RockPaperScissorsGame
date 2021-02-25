using System;
using System.Threading.Tasks;
using Client.Menus.Interfaces;
using Client.Models.Interfaces;
using Client.Services;

namespace Client.Menus
{
    internal class MainMenu : IMainMenu
    {
        private readonly IAccount _playerAccount;
        private readonly IRoomMenu _roomMenu;
        private readonly ILoginMenu _loginMenu;

        public MainMenu(
            IAccount playerAccount,
            IRoomMenu roomMenu,
            ILoginMenu loginMenu)
        {
            _playerAccount = playerAccount;
            _roomMenu = roomMenu;
            _loginMenu = loginMenu;
        }

        public async Task Start()
        {
            while (true)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor($"***\nHello, {_playerAccount.Login}\n" +
                                                                        "Please choose option", ConsoleColor.Cyan);
                ColorTextWriterService.PrintLineMessageWithSpecialColor("1.\tPlay with bot\n" +
                                                                        "2\tCreate room\n" +
                                                                        "3\tJoin Private room\n" +
                                                                        "4\tJoin Public room\n" +
                                                                        "5\tLog out", ConsoleColor.Yellow);

                ColorTextWriterService.PrintLineMessageWithSpecialColor("\nPlease select an item from the list", ConsoleColor.Green);

                Console.Write("Select -> ");
                var passed = int.TryParse(Console.ReadLine(), out int playersMenuInput);
                if (!passed)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Unsupported input", ConsoleColor.Red);
                    continue;
                }
                switch (playersMenuInput)
                {
                    case 1:
                        //todo: play with bot
                        break;
                    case 2:
                        await _roomMenu.CreateRoom();
                        break;
                    case 3:
                        await _roomMenu.JoinPrivateRoom();
                        break;
                    case 4:
                        await _roomMenu.JoinPublicRoom();
                        return;
                    case 5:
                        await _loginMenu.Logout();
                        break;
                    default:
                        ColorTextWriterService.PrintLineMessageWithSpecialColor("Unsupported input", ConsoleColor.Red);
                        continue;
                }
            }
        }
    }
}