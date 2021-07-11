namespace Server.Bll.Exceptions
{
    public class UserNotFoundException : LoginErrorException
    {
        public UserNotFoundException()
        {
        }
        public UserNotFoundException(string message) :
            base($"This user is not found!")
        {
        }
    }
}
