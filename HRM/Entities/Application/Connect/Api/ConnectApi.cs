namespace Entities.Application.Connect.Api
{
    public class ConnectApi
    {
        private string _Url { get; set; } = "https://localhost:44389";
        public string Url { get { return _Url; } set { _Url = value; } }
        //private string _Token { get; set; } = string.Empty;
        private string _Token { get; set; } = "";
        public string Token { get { return _Token; } set { _Token = value; } }
        private double _TimeOut { get; set; } = 5;
        public double TimeOut { get { return _TimeOut; } set { _TimeOut = value; } }
    }
}
