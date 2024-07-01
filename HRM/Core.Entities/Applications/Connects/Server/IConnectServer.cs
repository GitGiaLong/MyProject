namespace Core.Entities.Applications.Connects.Server
{
    public interface IConnectServer
    {
        string HostServerDB { get; set; }
        string PortServerDB { get; set; }
        string DatabaseName { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
