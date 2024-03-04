namespace Entities.Application.Convert
{

    public class ApiResponse : Paging, IRequest
    {
        public int TotalPages { get; set; } = 0;
        public int TotalRecords { get; set; } = 0;

        public bool Succeeded { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public int Status { get; set; } = 0;
    }
}
