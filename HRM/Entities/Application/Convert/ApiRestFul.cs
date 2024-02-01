using Entities.Application.Base;

namespace Entities.Application.Convert
{
    public class ApiRestFul<T> : Paging
    {
        public T? Data { get; set; } = default;
        //---
        public bool Succeeded { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public int Status { get; set; } = 0;

    }

}
