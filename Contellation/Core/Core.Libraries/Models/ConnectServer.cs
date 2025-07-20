using Core.Libraries.Extensions;

namespace Core.Libraries.Models
{
    public class ConnectServer : IConnectServer
    {

        XML xml = new XML();
        public ConnectServer()
        {
            if (!File.Exists($"{xml.FILE_PATH}.config"))
            {
                xml.createAndLoadXML();
            }
        }

        private string _HostServerDB = string.Empty;
        public string HostServerDB 
        { 
            get 
            {
                _HostServerDB = xml.ReadXML("Connect", "ServerName");
                return _HostServerDB; 
            } 
            set { _HostServerDB = value; } 
        }

        private static string _PortServerDB = string.Empty;
        public string PortServerDB { get { return _PortServerDB; } set { _PortServerDB = value; } }

        private static string _DatabaseName = string.Empty;
        public string DatabaseName 
        { 
            get
            {
                _DatabaseName = xml.ReadXML("Connect", "DatabaseName");
                return _DatabaseName; 
            } 
            set { _DatabaseName = value; }
        }

        private static string _Username = string.Empty;
        public string Username 
        { 
            get
            {
                _Username = xml.ReadXML("Connect", "Username");
                return _Username; 
            } 
            set { _Username = value; } 
        }

        private static string _Password = string.Empty;
        public string Password 
        { 
            get
            {
                _Password = xml.ReadXML("Connect", "Password");
                return _Password; 
            } 
            set { _Password = value; } 
        }
    }
}
