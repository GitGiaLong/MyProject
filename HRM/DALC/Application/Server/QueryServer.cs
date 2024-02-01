namespace DALC.Application.Server
{
    public class QueryServer
    {
        public string Query(string query)
        {
            return $"{query}";
        }
        public string SelectQuery(string query = "*")
        {
            return $"{query}";
        }
        public string PagingQuery(int page = 1, int pageSize = 10, string OrderBy = "IsOnly")
        {
            if(page > 0 && pageSize > 0)
            {
                return $"'ORDER BY {OrderBy} OFFSET {page} ROWS FETCH NEXT {pageSize} ROWS ONLY'";
            }
            else
            {
                return $"";
            }    
        }
    }
}
