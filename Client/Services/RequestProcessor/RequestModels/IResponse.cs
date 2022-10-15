namespace Client.Services.RequestProcessor.RequestModels;

public interface IResponse
{
    public bool Handled { get; }
    public int Code { get; }
    public string Content { get; }
}