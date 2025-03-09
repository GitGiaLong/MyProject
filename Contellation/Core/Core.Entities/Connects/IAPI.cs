namespace Core.Entities.Connects
{
    public interface IAPI
    {
        /// <summary>
        /// Link API
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        string Token { get; set; }

        /// <summary>
        /// Time Out
        /// </summary>
        double TimeOut { get; set; }
    }
}
