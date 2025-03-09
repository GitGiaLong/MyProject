using System.Collections.ObjectModel;
using System.Data;
using Core.Libraries.Models;

namespace Core.Libraries.Connects
{
    public interface IConnectServers
    {
        T GetTable<T>(string sql);
        DataTable GetDataTable(string sql);
        ObservableCollection<T> GetDataReader<T>(string sql);
        void ExecuteData(string sql);
        string Field(string sql);
        string Query(string query);
        string SelectQuery(IFilter filter, string database, string theWorld);
        string PagingSQLServers(int page, int pageSize, string OrderBy = "IsOnly");
    }
}
