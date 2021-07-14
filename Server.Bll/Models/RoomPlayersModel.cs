
namespace Server.Bll.Models
{
    public class RoomPlayersModel
    { 
        public AccountModel FirstPlayer { get; set; }
        public int FirstPlayerMove { get; set; }
        public AccountModel SecondPlayer { get; set; }
        public int SecondPlayerMove { get; set; }
    }
}