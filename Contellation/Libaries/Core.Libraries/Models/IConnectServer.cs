namespace Core.Libraries.Models
{
    /// <summary>
    /// Connect server 
    /// 1 sql server
    /// 2 My sql
    /// 3 oracal Db
    /// 4 MongoDB
    /// </summary>
    public interface IConnectServer
    {
        /// <summary>
        /// Server name (Server name, Host name)
        /// </summary>
        public string HostServerDB { get; set; }

        /// <summary>
        /// Port server name
        /// </summary>
        public string PortServerDB { get; set; }

        /// <summary>
        /// Data base in server
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
