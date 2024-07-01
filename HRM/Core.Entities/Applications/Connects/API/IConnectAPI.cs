namespace Core.Entities.Applications.Connects.API
{
    public interface IConnectAPI
    {
        string Url { get; set; }
        string Token { get; set; }
        double TimeOut { get; set; }
    }
}
