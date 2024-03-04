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
        public string PagingQuery(int page, int pageSize, string OrderBy = "IsOnly")
        {

            if(page > 0 && pageSize > 0)
            {
                return $"'ORDER BY {OrderBy} OFFSET {pageSize * (page - 1) } ROWS FETCH NEXT {pageSize} ROWS ONLY'";
            }
            else
            {
                return $"";
            }    
        }
    }
}
