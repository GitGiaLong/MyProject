namespace Core.Library.Extensions
{
    public class ServerException : Exception
    {
        public ServerException() : base()
        {
        }
        public ServerException(string message) : base(message)
        {
        }
    }
}
