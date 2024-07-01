using System.Net;

namespace Core.Entities.Applications.Base.Responses
{
    public class Response : Paging

    {
        public bool _Succeeded = false;
        /// <summary>
        /// Succeeded
        /// </summary>
        public bool Succeeded { get; set; } = false;

        public string _Message = string.Empty;
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        public int _Status = (int)HttpStatusCode.Unused;
        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; } = 0;
    }
}
