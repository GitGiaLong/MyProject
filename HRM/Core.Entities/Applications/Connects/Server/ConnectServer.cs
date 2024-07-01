namespace Core.Entities.Applications.Connects.Server
{
    /// <summary>
    /// Connect server 
    /// 1 sql server
    /// 2 My sql
    /// 3 oracal Db
    /// 4 MongoDB
    /// </summary>
    public class ConnectServer : IConnectServer
    {
        private string _HostServerDB = string.Empty;
        /// <summary>
        /// server name (Server name, Host name)
        /// </summary>
        public string HostServerDB { get { return _HostServerDB; } set { _HostServerDB = value; } }

        private static string _PortServerDB = string.Empty;
        /// <summary>
        /// Port server name
        /// </summary>
        public string PortServerDB { get { return _PortServerDB; } set { _PortServerDB = value; } }

        private static string _DatabaseName = string.Empty;
        /// <summary>
        /// Data base in server
        /// </summary>
        public string DatabaseName { get { return _DatabaseName; } set { _DatabaseName = value; } }

        private static string _Username = string.Empty;
        /// <summary>
        /// User name
        /// </summary>
        public string Username { get { return _Username; } set { _Username = value; } }

        private static string _Password = string.Empty;
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get { return _Password; } set { _Password = value; } }
    }
}
