namespace Server.Bll.Exceptions
{
    public class AlreadyExistsException : UnknownReasonException
    {
        public AlreadyExistsException()
        {
            
        }

        public AlreadyExistsException(string message) : base(string.Format($"This account already exists"))
        {
            
        }
    }
}