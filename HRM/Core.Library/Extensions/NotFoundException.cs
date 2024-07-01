namespace Core.Library.Extensions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {
        }
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
