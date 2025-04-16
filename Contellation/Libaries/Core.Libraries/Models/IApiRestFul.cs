namespace Core.Libraries.Models
{
    public interface IApiRestFul<T>
    {
        T? Data { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
        int TotalPages { get; set; }
        int TotalRecords { get; set; }
        bool Succeeded { get; set; }
        string Message { get; set; }
        int Status { get; set; }
    }
}
