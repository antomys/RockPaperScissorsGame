namespace Server.Exceptions.LogIn
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