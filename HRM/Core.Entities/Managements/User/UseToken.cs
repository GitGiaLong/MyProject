namespace Core.Entities.Managements.User
{
    public class UseToken : User
    {
        private string _Token = string.Empty;
        public string Token { get { return _Token; } set { _Token = value; } }
    }
}
