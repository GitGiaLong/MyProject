namespace Core.Entities.Connects
{
    public class Server : Account
    {

        private string _HostServerDB = string.Empty;
        public string HostServerDB { get { return _HostServerDB; } set { _HostServerDB = value; } }

        private static string _PortServerDB = string.Empty;
        public string PortServerDB { get { return _PortServerDB; } set { _PortServerDB = value; } }

        private static string _DatabaseName = string.Empty;
        public string DatabaseName { get { return _DatabaseName; } set { _DatabaseName = value; } }

    }
}
