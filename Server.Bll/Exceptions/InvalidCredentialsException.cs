namespace Server.Bll.Exceptions
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