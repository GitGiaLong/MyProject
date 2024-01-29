﻿namespace Entities.Application.Convert
{
    public class ApiRestFul<T>
    {
        public T? Data { get; set; } = default;
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? TotalPages { get; set; }
        public int? TotalRecords { get; set; }
        //---
        public bool Succeeded { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public int Status { get; set; } = 0;

    }

}