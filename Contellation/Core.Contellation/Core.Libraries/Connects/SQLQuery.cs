using Core.Libraries.Models;

namespace Core.Libraries.Connects
{
    public class SQLQuery
    {
        public string Query(string query)
        {
            return $"{query}";
        }

        public string SelectQuery(IFilter filter, string database, string table)
        {
            string query = $"SELECT {filter.Select} FROM {database}..{table} {filter.Value} {PagingSQLServers(filter.Page, filter.PageSize)}";
            return $"{query}";
        }

        public string PagingSQLServers(int page, int pageSize, string OrderBy = "IsOnly")
        {

            if (page > 0 && pageSize > 0)
            {
                return $"ORDER BY {OrderBy} OFFSET {pageSize * (page - 1)} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            }
            else
            {
                return $"";
            }
        }
    }
}
