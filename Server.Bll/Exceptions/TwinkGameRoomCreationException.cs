using System;

namespace Server.Bll.Exceptions
{
    public class TwinkGameRoomCreationException:Exception
    {
        public TwinkGameRoomCreationException() :
            base($"Failed to creaate one more game when you are sitting in another room!")
        {
        }
    }
}
