namespace Entities.Application.Connect.Server
{
    /// <summary>
    /// Connect server 
    /// 1 sql server
    /// 2 My sql
    /// oracal Db
    /// </summary>
    public class ConnectServer
    {
        private string _HostServerDB = "";
        /// <summary>
        /// server name (Server name, Host name)
        /// </summary>
        public string HostServerDB { get { return _HostServerDB; } set { _HostServerDB = value; } }

        private string _PortServerDB = "";
        /// <summary>
        /// Port server name
        /// </summary>
        public string PortServerDB { get { return _PortServerDB; } set { _PortServerDB = value; } }

        private string _DatabaseName = "";
        /// <summary>
        /// Data base in server
        /// </summary>
        public string DatabaseName { get { return _DatabaseName; } set { _DatabaseName = value; } }

        private string _Username = "";
        /// <summary>
        /// User name
        /// </summary>
        public string Username { get { return _Username; } set { _Username = value; } }

        private string _Password = "";
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get { return _Password; } set { _Password = value; } }
    }
}
