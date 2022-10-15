/*using System;
using System.Threading.Tasks;
using Client.Host.Models;
using Client.Host.Services;
using Client.Host.Services.RequestProcessor;

namespace Client.Host.Menus;

public class MainMenu : IMainMenu
{
    private readonly TokenModel _playerAccount;
    private readonly IRoomService _roomService;
    private readonly IStatisticsService _statisticsService;

    public MainMenu(TokenModel account, 
        IRequestPerformer requestPerformer, 
        IStatisticsService statisticsService)
    {
        _playerAccount = account;
        _statisticsService = statisticsService;
        _roomService = new RoomService(_playerAccount, requestPerformer);
    }

    public async Task PlayerMenu()
    {
        while (true)
        {
            TextWrite.Print(
                $"***\nHello, {_playerAccount.Login}\n" + 
                "Please choose option", ConsoleColor.Cyan);
            TextWrite.Print(
                "1.\tPlay with bot\n" +
                "2\tCreate room\n" + 
                "3\tJoin room\n" + 
                "4\tSearch open room\n" + 
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
            switch (playersMenuInput)
            {
                case 1:
                    Console.Clear();
                    //await JoinRoomWithBot();
                    var room = await _roomService.CreateRoom(true, true);
                    if (room is null)
                        return;
                    //todo: redirect somewhere
                    break;
                case 2:
                    Console.Clear();
                    //await CreationRoom();
                    break;
                case 3:
                    Console.Clear();
                    //await JoinPrivateRoom();
                    break;
                case 4:
                    Console.Clear();
                    //await JoinPublicRoom();
                    break;
                case 5:
                    Console.Clear();
                    var statistics = await _statisticsService
                        .GetPersonalStatistics(_playerAccount.BearerToken);
                    Console.WriteLine(statistics+"\n\nPress any key to go back.");
                    Console.ReadKey();
                    break;
                case 6:
                    Console.Clear();
                    //await Logout();
                    return;
                default:
                    TextWrite.Print("Unsupported input", ConsoleColor.Red);
                    continue;
            }
        }
    }
}*/