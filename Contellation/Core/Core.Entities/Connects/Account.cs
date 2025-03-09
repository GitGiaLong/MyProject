namespace Core.Entities.Connects
{
    public class Account
    {

        private string _Username = string.Empty;
        public string Username { get { return _Username; } set { _Username = value; } }

        private string _Password = string.Empty;
        public string Password { get { return _Password; } set { _Password = value; } }
    }
}
