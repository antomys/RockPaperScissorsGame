namespace Client.Services.RequestModels
{
    public interface IResponse
    {
        public bool Handled { get; }
        public int Code { get; }
        public string Content { get; }
    }
}