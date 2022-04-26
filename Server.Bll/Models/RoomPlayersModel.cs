
namespace Server.Bll.Models;

public sealed class RoomPlayersModel
{ 
    public AccountModel FirstPlayer { get; set; }
    
    public int FirstPlayerMove { get; set; }
    
    public AccountModel SecondPlayer { get; set; }
    
    public int SecondPlayerMove { get; set; }
}