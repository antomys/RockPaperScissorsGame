using System;
using System.Threading.Tasks;
using Client.Models.Interfaces;
using Client.Services;

namespace Client.Menus
{
    public class MainMenu : IMainMenu
    {
        private readonly IAccount _playerAccount;

        public MainMenu(IAccount account)
        {
            _playerAccount = account;
        }

        public async Task PlayerMenu()
        {
            while (true)
            {
                TextWrite.Print($"***\nHello, {_playerAccount.Login}\n" + 
                                             "Please choose option", ConsoleColor.Cyan);
                TextWrite.Print("1.\tPlay with bot\n" +
                                             "2\tCreate room\n" +
                                             "3\tJoin Private room\n" + 
                                             "4\tJoin Public room\n" + 
                                             "5\tShow Statistics\n" + 
                                             "6\tLog out", ConsoleColor.Yellow);
        
                TextWrite.Print("\nPlease select an item from the list", ConsoleColor.Green);
        
                Console.Write("Select -> ");
                var passed = int.TryParse(Console.ReadLine(), out var playersMenuInput);
                if (!passed)
                {
                    TextWrite.Print("Unsupported input", ConsoleColor.Red);
                    continue;
                }
                /*switch (playersMenuInput)
                {
                    case 1:
                        Console.Clear();
                        await JoinRoomWithBot();
                        break;
                    case 2:
                        Console.Clear();
                        await CreationRoom();
                        break;
                    case 3:
                        Console.Clear();
                        await JoinPrivateRoom();
                        break;
                    case 4:
                        Console.Clear();
                        await JoinPublicRoom();
                        break;
                    case 5:
                        Console.Clear();
                        var statistics = await PersonalStatistics(_sessionId);
                        Console.WriteLine(statistics+"\n\nPress any key to go back.");
                        Console.ReadKey();
                        break;
                    case 6:
                        Console.Clear();
                        await Logout();
                        return;
                    default:
                        TextWrite.PrintLineMessageWithSpecialColor("Unsupported input", ConsoleColor.Red);
                        continue;
                }*/
            }
        }
    }
}