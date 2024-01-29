using DALC.Application.Database;
using DALC.Application.Procedure;
using DALC.Application.Server;
using Entities.Application.Connect.Api;
using Entities.Catelogies.TheWorld;
using Entities.Catelogies.TheWorld.Country;
using GSMF.Extensions;
using System.Collections.ObjectModel;

namespace DALC.Catelogies.TheWorld
{
    public class DALTheWorld : ConnectFromServer
    {
        public DALTheWorld() { }

        public ObservableCollection<T> GetCountry<T>(EnumTheWorld Value)
        {
            //ObservableCollection<EntityCountry> data = new ObservableCollection<EntityCountry>();
            return GetDataReader<T>($"EXEC {EnumProcedure.ViewOrSelectQuery.ToEnumString()} '{EnumDatabse.TheWorld.ToEnumString()}..{Value.ToEnumString()}'");
        }
        public void ActionCountry(EnumMethodApi type, EnumTheWorld Value, object Data)
        {
            switch (type)
            {
                case EnumMethodApi.B:
                    ExecuteData($"EXEC {EnumProcedure.InsertValueQuery.ToEnumString()} '{EnumDatabse.TheWorld.ToEnumString()}..{Value.ToEnumString()}', {QueryTheWorld(Value, Data)}");
                    break;
                case EnumMethodApi.C:
                    //ExecuteData($"EXEC UpdateValueQuery 'TheWorld..Country',N'Code =''{value.Code}'', NameEN = ''{value.NameEN}'' , NameVI = N''{value.NameVI}'' , UpdateOn= GETDATE(), UpdateBy = ''{value.UpdateBy}''',N'IsOnly = ''{value.IsOnly}'''");
                    break;
                case EnumMethodApi.D:
                    //ExecuteData($"EXEC DeleteQuery 'TheWorld..Country', N'IsOnly =''{value.IsOnly}'''");
                    break;
            }
        }

        private string QueryTheWorld(EnumTheWorld Value, object value)
        {
            string query = "";
            switch (Value)
            {
                case EnumTheWorld.Country:
                    var a = (EntityCountry)value;
                    query = $"N'''{a.IsOnly}'',''{a.Code}'',''{a.NameEN}'',N''{a.NameVI}'',''{a.IsCurently}'',GETDATE(),''{a.CreateBy}'',GETDATE(),''{a.UpdateBy}'''";
                    break;
            }

            return query;
        }
    }
}
