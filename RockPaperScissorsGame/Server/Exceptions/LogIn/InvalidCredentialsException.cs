namespace Server.Exceptions.LogIn
{
    public class InvalidCredentialsException : LoginErrorException
    {
        protected InvalidCredentialsException()
        {
            
        }

        public InvalidCredentialsException(string message) :
            base($"Invalid username of password!")
        {
            
        }
        
    }
}