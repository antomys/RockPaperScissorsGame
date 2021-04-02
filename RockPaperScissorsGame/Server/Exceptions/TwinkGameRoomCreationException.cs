using System;

namespace Server.GameLogic.Exceptions
{
    public class TwinkGameRoomCreationException:Exception
    {
        public TwinkGameRoomCreationException() :
            base($"Failed to creaate one more game when you are sitting in another room!")
        {
        }
    }
}
