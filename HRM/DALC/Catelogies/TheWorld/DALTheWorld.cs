using DALC.Application.Database;
using DALC.Application.Procedure;
using DALC.Application.Server;
using Entities.Application.Connect.Api;
using Entities.Catelogies.Countries;
using Entities.Catelogies.TheWorld;
using Entities.Catelogies.TheWorld.Distrist;
using Entities.Catelogies.TheWorld.Provoice;
using Entities.Catelogies.TheWorld.Town;
using GSMF.Extensions;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace DALC.Catelogies.TheWorld
{
    public class DALTheWorld : ConnectFromServer
    {

        DisplayName display = new DisplayName();
        ConvertClass Convert = new ConvertClass();
        public DALTheWorld() { }

        public ObservableCollection<T> GetTheWorld<T>(EnumTheWorld Value, EntityTheWorld code)
        {
            string query = $"EXEC {EnumProcedure.ViewOrSelectQuery.ToEnumString()} '{EnumDatabse.TheWorld.ToEnumString()}..{Value.ToEnumString()}'";
            string sub = "";
            if (code.value != null)
            {
                if (Value == EnumTheWorld.Country)
                {
                    var a = Convert.ConvertClassToObject<EntityCountry>(code.value);
                    sub += $"{Query($"'WHERE {display.Display(() => a.IsOnly, false)} LIKE ''%{a.IsOnly}%'' AND {display.Display(() => a.Code, false)} LIKE ''%{a.Code}%'' AND {display.Display(() => a.NameEN, false)} LIKE ''%{a.NameEN}%''AND {display.Display(() => a.NameVI, false)} LIKE ''%{a.NameVI}%'''")}";
                }
                else if (Value == EnumTheWorld.Provoice)
                {
                    var a = Convert.ConvertClassToObject<EntityProvoice>(code.value);
                    sub += $"{Query($"'WHERE {display.Display(() => a.IsOnly, false)} LIKE ''%{a.IsOnly}%'' AND {display.Display(() => a.CodeCountry, false)} LIKE ''%{a.CodeCountry}%'' AND {display.Display(() => a.DisplayName, false)} LIKE ''%{a.DisplayName}%'''")}";
                }
                else if (Value == EnumTheWorld.Distrist)
                {
                    var a = Convert.ConvertClassToObject<EntityDistrist>(code.value);
                    sub += $"{Query($"'WHERE {display.Display(() => a.IsOnly, false)} LIKE ''%{a.IsOnly}%'' AND {display.Display(() => a.CodeProvoice, false)} LIKE ''%{a.CodeProvoice}%'' AND {display.Display(() => a.DisplayName, false)} LIKE ''%{a.DisplayName}%'''")}";
                }
                else if (Value == EnumTheWorld.Town)
                {
                    var a = Convert.ConvertClassToObject<EntityTown>(code.value);
                    sub += $"{Query($"'WHERE {display.Display(() => a.IsOnly, false)} LIKE ''%{a.IsOnly}%'' AND {display.Display(() => a.CodeDistrist, false)} LIKE ''%{a.CodeDistrist}%'' AND {display.Display(() => a.DisplayName, false)} LIKE ''%{a.DisplayName}%'''")}";
                }
            }
            return GetDataReader<T>($"{query},{(code.value != null ? sub : PagingQuery(code.PageNumber, code.PageSize))}");
        }
        public void ActionCountry(EnumMethodApi type, EnumTheWorld Value, object Data)
        {
            switch (type)
            {
                case EnumMethodApi.B:
                    ExecuteData($"EXEC {EnumProcedure.InsertValueQuery.ToEnumString()} '{EnumDatabse.TheWorld.ToEnumString()}..{Value.ToEnumString()}', {QueryTheWorld(type, Value, Data)}");
                    break;
                case EnumMethodApi.C:
                    ExecuteData($"EXEC {EnumProcedure.UpdateValueQuery.ToEnumString()} '{EnumDatabse.TheWorld.ToEnumString()}..{Value.ToEnumString()}', {QueryTheWorld(type, Value, Data)}");
                    break;
                case EnumMethodApi.D:
                    ExecuteData($"EXEC {EnumProcedure.DeleteQuery.ToEnumString()} '{EnumDatabse.TheWorld.ToEnumString()}..{Value.ToEnumString()}', {QueryTheWorld(type, Value, Data)}");
                    break;
            }
        }

        private string QueryTheWorld(EnumMethodApi type, EnumTheWorld Value, object Data)
        {
            string query = "";
            if (Value == EnumTheWorld.Country)
            {
                EntityCountry a = JsonConvert.DeserializeObject<EntityCountry>(Data.ToString());
                switch (type)
                {
                    case EnumMethodApi.B:
                        query = $"N'''{a.IsOnly}'',''{a.Code}'',''{a.NameEN}'',N''{a.NameVI}'',''{a.IsCurently}'',GETDATE(),''{a.CreateBy}'',GETDATE(),''{a.UpdateBy}'''";
                        break;
                    case EnumMethodApi.C:
                        query = $"N'{display.Display(() => a.Code, false)} =''{a.Code}'', {display.Display(() => a.NameEN, false)} = ''{a.NameEN}'' , {display.Display(() => a.NameVI, false)} = N''{a.NameVI}'' , {display.Display(() => a.UpdateOn, false)} = GETDATE(), {display.Display(() => a.UpdateBy, false)} = ''{a.UpdateBy}''',N'{display.Display(() => a.IsOnly, false)} = ''{a.IsOnly}'''";
                        break;
                    case EnumMethodApi.D:
                        query = $"N'{display.Display(() => a.IsOnly, false)} =''{a.IsOnly}'''";
                        break;
                }
            }
            else if (Value == EnumTheWorld.Provoice)
            {
                EntityProvoice a = JsonConvert.DeserializeObject<EntityProvoice>(Data.ToString());
                switch (type)
                {
                    case EnumMethodApi.B:
                        query = $"N'''{a.IsOnly}'',''{a.CodeCountry}'',''{a.DisplayName}'',''{a.IsCurently}'',GETDATE(),''{a.CreateBy}'',GETDATE(),''{a.UpdateBy}'''";
                        break;
                    case EnumMethodApi.C:
                        query = $"N'{display.Display(() => a.CodeCountry, false)} =''{a.CodeCountry}'',{display.Display(() => a.DisplayName, false)} =''{a.DisplayName}'', {display.Display(() => a.UpdateOn, false)}= GETDATE(), {display.Display(() => a.UpdateBy, false)} = ''{a.UpdateBy}''',N'{display.Display(() => a.IsOnly, false)} = ''{a.IsOnly}'''";
                        break;
                    case EnumMethodApi.D:
                        query = $"N'{display.Display(() => a.IsOnly, false)} =''{a.IsOnly}'''";
                        break;
                }
            }
            else if (Value == EnumTheWorld.Distrist)
            {
                EntityDistrist a = JsonConvert.DeserializeObject<EntityDistrist>(Data.ToString());
                switch (type)
                {
                    case EnumMethodApi.B:
                        query = $"N'''{a.IsOnly}'',''{a.CodeProvoice}'',''{a.DisplayName}'',''{a.IsCurently}'',GETDATE(),''{a.CreateBy}'',GETDATE(),''{a.UpdateBy}'''";
                        break;
                    case EnumMethodApi.C:
                        query = $"N'{display.Display(() => a.CodeProvoice, false)} =''{a.CodeProvoice}'',{display.Display(() => a.DisplayName, false)} =''{a.DisplayName}'', {display.Display(() => a.UpdateOn, false)}= GETDATE(), {display.Display(() => a.UpdateBy, false)} = ''{a.UpdateBy}''',N'{display.Display(() => a.IsOnly, false)} = ''{a.IsOnly}'''";
                        break;
                    case EnumMethodApi.D:
                        query = $"N'{display.Display(() => a.IsOnly, false)} =''{a.IsOnly}'''";
                        break;
                }
            }
            else if (Value == EnumTheWorld.Town)
            {
                EntityTown a = JsonConvert.DeserializeObject<EntityTown>(Data.ToString());
                switch (type)
                {
                    case EnumMethodApi.B:
                        query = $"N'''{a.IsOnly}'',''{a.CodeDistrist}'',''{a.DisplayName}'',''{a.IsCurently}'',GETDATE(),''{a.CreateBy}'',GETDATE(),''{a.UpdateBy}'''";
                        break;
                    case EnumMethodApi.C:
                        query = $"N'{display.Display(() => a.CodeDistrist, false)} =''{a.CodeDistrist}'',{display.Display(() => a.DisplayName, false)} =''{a.DisplayName}'', {display.Display(() => a.UpdateOn, false)}= GETDATE(), {display.Display(() => a.UpdateBy, false)} = ''{a.UpdateBy}''',N'{display.Display(() => a.IsOnly, false)} = ''{a.IsOnly}'''";
                        break;
                    case EnumMethodApi.D:
                        query = $"N'{display.Display(() => a.IsOnly, false)} =''{a.IsOnly}'''";
                        break;
                }
            }

            return query;
        }


    }
}
