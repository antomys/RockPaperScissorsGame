namespace Server.Bll.Exceptions
{
    public readonly struct RoomException
    {
        public int Code { get;}
        public string Message { get; }

        public RoomException(string message, int code, string template)
        {
            Message = string.Format(template,message);
            Code = code;
        }
    }
}