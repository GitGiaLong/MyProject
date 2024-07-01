using Core.Entities.Applications.Base.Filters;
using System.Collections.ObjectModel;
using System.Data;

namespace Core.Library.Applications.Server
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
