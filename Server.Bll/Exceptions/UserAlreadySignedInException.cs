namespace Server.Exceptions.LogIn
{
    public class UserAlreadySignedInException : LoginErrorException
    {
        public UserAlreadySignedInException()
        {
            
        }

        public UserAlreadySignedInException(string message) : base(string.Format($"Already signed in!"))
        {
            
        }
    }
}