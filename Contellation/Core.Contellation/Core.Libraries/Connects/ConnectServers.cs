using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Core.Libraries.Extensions;
using Core.Libraries.Models;

namespace Core.Libraries.Connects
{
    public class ConnectServers : SQLQuery, IConnectServers
    {
        //String ConnectServers = $"Server = ROYALDRAGON\\ROYALDRAGON_2012; Database = HRM; trusted_connection= true";
        SqlDataAdapter da;
        DataTable dt;
        private ConnectServer Server { get; set; } = new ConnectServer();
        private string StringConnectServer()
        {

            string StringConnectServer =
                $"Server = {Server.HostServerDB.Trim()}; " +
                $"Database = {Server.DatabaseName.Trim()}; " +
                $"trusted_connection= true";
            return StringConnectServer;
        }

        public T GetTable<T>(string sql)
        {
            SqlConnection cnn = new SqlConnection(StringConnectServer());
            cnn.Open();
            SqlCommand cmd = new SqlCommand(sql, cnn);
            var rows = (T)cmd.ExecuteScalar();
            cnn.Close();
            return rows;
        }

        public DataTable GetDataTable(string sql)
        {
            da = new SqlDataAdapter(sql, StringConnectServer());
            dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public ObservableCollection<T> GetDataReader<T>(string sql)
        {
            ObservableCollection<T> data = new ObservableCollection<T>();
            T obj = default;
            using (SqlConnection con = new SqlConnection(StringConnectServer()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obj = Activator.CreateInstance<T>();
                            foreach (PropertyInfo prop in obj.GetType().GetProperties())
                            {
                                if (!Equals(reader[prop.Name], DBNull.Value))
                                {
                                    prop.SetValue(obj, reader[prop.Name], null);
                                }
                            }
                            data.Add(obj);
                        }
                    }
                }
            }
            return data;
        }
        public List<T> GetDataTable<T>(string sql)
        {
            List<T> data = new List<T>();
            GetDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public void ExecuteData(string sql)
        {
            SqlConnection cnn = new SqlConnection(StringConnectServer());
            cnn.Open();
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public string Field(string sql)
        {
            SqlConnection cnn = new SqlConnection(StringConnectServer());
            cnn.Open();
            SqlCommand cmd = new SqlCommand(sql, cnn);

            return cmd.ExecuteScalar().ToString();
        }
    }
}
