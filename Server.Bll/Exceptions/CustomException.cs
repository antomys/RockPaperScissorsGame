namespace Server.Bll.Exceptions
{
    public readonly struct CustomException
    {
        public int Code { get;}
        public string Message { get; }

        public CustomException(string template, int code = 400)
        {
            Message = template;
            Code = code;
        }

        public CustomException(string template, string customObject, int code = 400)
        {
            Message = string.Format(template, customObject);
            Code = code;
        }
    }
}