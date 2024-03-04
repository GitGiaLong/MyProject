using Entities.Application.Convert;
using System.Net;

namespace WebAPICsharp.Extensions
{
    public class ResponseResult
    {
        public ApiRestFul<T> ResultOk<T>(T data, int page, int pageSize,int totalRecords)
        {
            ApiRestFul<T> result = new ApiRestFul<T>();
            result.Data = data;

            result.Status = (int)HttpStatusCode.OK;
            result.Succeeded = true;
            result.Page = page;
            result.PageSize = pageSize;
            result.TotalRecords = totalRecords;
            result.TotalPages = totalRecords % pageSize != 0 ? totalRecords / pageSize + 1 : totalRecords / pageSize;
            return result;
        }

        public IRequest ResultBad(string message)
        {
            ApiResponse result = new ApiResponse();
            result.Message = "Lỗi";
            result.Status = (int)HttpStatusCode.BadRequest;
            result.Succeeded = false;
            return result;
        }

        public IRequest ResultNotFound()
        {
            ApiResponse result = new ApiResponse();
            result.Message = "Không tìm thấy dữ liệu";
            result.Status = (int)HttpStatusCode.NotFound;
            result.Succeeded = false;
            return result;
        }
    }
}
