namespace Client.Services.RequestModels
{
    public interface IRequestOptions
    {
        string Name { get; }
        string Address { get; }
        RequestMethod Method { get; }
        string ContentType { get; }
        string Body { get; }
        bool IsValid { get; }
    }
}
