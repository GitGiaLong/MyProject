using DALC.Application.Database;
using Entities.Application.Connect.Server;
using GSMF.Application.Local.Extensions;
using GSMF.Extensions;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace DALC.Application.Server
{
    public class ConnectFromServer : QueryServer
    {

        XMLExtension xMLExtension = new XMLExtension(@"..\\DALC\\Application\\Local\\ConnectServer.xml");
        private ConnectServer connectDB = new ConnectServer();
        //String ConnectServers = $"Server = ROYALDRAGON\\ROYALDRAGON_2012; Database = HRM; trusted_connection= true";
        SqlDataAdapter da;
        DataTable dt;
        public String ConnectServer(string? serverName = null, EnumDatabse database = EnumDatabse.HRM, string? user = null, string? pass = null)
        {
            String ConnectServer = 
                $"Server = {(!string.IsNullOrEmpty(serverName) ? serverName : xMLExtension.GetAttributesXML("Connect", "serverName"))}; " +
                $"Database = {database.ToEnumString()}; " +
                $"trusted_connection= true";
            return ConnectServer;
        }
        public T GetTable<T>(string sql)
        {
            SqlConnection cnn = new SqlConnection(ConnectServer());
            cnn.Open();
            SqlCommand cmd = new SqlCommand(sql, cnn);
            var rows = (T)cmd.ExecuteScalar();
            cnn.Close();
            return rows;
        }
        public DataTable GetDataTable(string sql)
        {
            da = new SqlDataAdapter(sql, ConnectServer());
            dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public ObservableCollection<T> GetDataReader<T>(string sql)
        {
            ObservableCollection<T> data = new ObservableCollection<T>();
            T obj = default(T);
            using (SqlConnection con = new SqlConnection(ConnectServer()))
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
                                if (!object.Equals(reader[prop.Name], DBNull.Value))
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
        public ObservableCollection<T> GetDataTable<T>(string sql)
        {
            ObservableCollection<T> data = new ObservableCollection<T>();
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
            SqlConnection cnn = new SqlConnection(ConnectServer());
            cnn.Open();
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public string field(string sql)
        {
            SqlConnection cnn = new SqlConnection(ConnectServer());
            cnn.Open();
            SqlCommand cmd = new SqlCommand(sql, cnn);

            return cmd.ExecuteScalar().ToString();
        }
    }
}
