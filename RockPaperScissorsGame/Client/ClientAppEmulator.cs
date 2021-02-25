using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Menus;
using Client.Menus.Interfaces;
using Client.Models;
using Client.Models.Interfaces;
using Client.Services;
using Client.Services.RequestProcessor;

namespace Client
{
    public class ClientAppEmulator
    {
        private readonly string _sessionId = Guid.NewGuid().ToString();
        private const string BaseAddress = "http://localhost:5000/";
        private readonly IStartMenu _startMenu;

        

        public ClientAppEmulator(IRequestPerformer performer)
        {
            var playerAccount = new Account(); 
            var room = new Room();
            var round = new Round(); 
            var statistics = new Statistics();

            IRoomMenu roomMenu = new RoomMenu(_sessionId,BaseAddress,room,performer);
            IStatisticsMenu statisticsMenu = new StatisticsMenu(BaseAddress,performer);
            ILoginMenu loginMenu = new LoginMenu(_sessionId,BaseAddress,performer);
            IRegisterMenu registerMenu = new RegisterMenu(_sessionId,BaseAddress,playerAccount,performer);
            IMainMenu mainMenu = new MainMenu(playerAccount,roomMenu,loginMenu);
            _startMenu = new StartMenu(loginMenu,mainMenu,statisticsMenu,registerMenu,playerAccount);
        }
        
        public async Task<int> StartAsync()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            try
            {
                Greeting();
                ColorTextWriterService.PrintLineMessageWithSpecialColor("\n\nPress any key to continue..."
                    , ConsoleColor.Green);
                Console.ReadKey();
                Console.Clear();
                await _startMenu.Start(token);
                return 1;
            }
            catch(Exception exception)
            {
                Console.WriteLine($"Exception occured, ClientAppEmulator, {exception.Message}");
            }

            return -1;
        }
        private static void Greeting()
        {
            ColorTextWriterService.PrintLineMessageWithSpecialColor(
                    "Welcome to the world best game ----> Rock-Paper-Scissors!\n" +
                    "You are given the opportunity to compete with other users in this wonderful game,\n" +
                    "or if you don’t have anyone to play, don’t worry,\n" +
                    "you can find a random player or just try your skill with a bot.", ConsoleColor.Yellow);
            ColorTextWriterService.PrintLineMessageWithSpecialColor("(c)Ihor Volokhovych & Michael Terekhov", ConsoleColor.Cyan);
        }
    }
}

        

