using System.Collections.Generic;

namespace Server.Bll.Models
{
    public class RoomPlayersModel
    {
        public RoomModel Room { get; set; }

        public ICollection<AccountModel> Accounts { get; set; }

        public int FirstPlayerMove { get; set; }

        public int SecondPlayerMove { get; set; }
        
        public RoundModel Round { get; set; }
    }
}