namespace Client.Services.RequestModels
{
    internal interface IResponse
    {
        public bool Handled { get; }
        public int Code { get; }
        public string Content { get; }
    }
}