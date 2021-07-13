namespace Server.Bll.Exceptions
{
    public readonly struct RoomException
    {
        public int Code { get;}
        public string Message { get; }

        public RoomException(string template, int code)
        {
            Message = template;
            Code = code;
        }
    }
}