
namespace Server.Exceptions.LogIn
{
    public class LoginCooldownException : LoginErrorException
    {
        protected LoginCooldownException()
        {
            
        }

        public LoginCooldownException(string message, int afterTime) :
            base($"You got a cooldown! Try in {afterTime} seconds")
        {
            
        }
    }
}