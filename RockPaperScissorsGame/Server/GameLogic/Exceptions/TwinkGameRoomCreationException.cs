using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.GameLogic.Exceptions
{
    public class TwinkGameRoomCreationException:Exception
    {
        public TwinkGameRoomCreationException() :
            base($"This user is not found!")
        {
        }
    }
}
