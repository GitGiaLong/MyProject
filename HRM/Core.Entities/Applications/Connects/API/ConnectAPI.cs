namespace Core.Entities.Applications.Connects.API
{
    public class ConnectAPI
    {
        private string _Url { get; set; } = "https://localhost:44388/";
        /// <summary>
        /// Link API
        /// </summary>
        public string Url { get { return _Url; } set { _Url = value; } }

        private string _Token { get; set; } = string.Empty;
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get { return _Token; } set { _Token = value; } }

        private double _TimeOut { get; set; } = 5;
        /// <summary>
        /// Time Out
        /// </summary>
        public double TimeOut { get { return _TimeOut; } set { _TimeOut = value; } }
    }
}
