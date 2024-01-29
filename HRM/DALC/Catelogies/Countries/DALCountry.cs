using DALC.Application.Server;
using Entities.Application.Connect.Api;
using Entities.Catelogies.Countries;
using System.Collections.ObjectModel;

namespace DALC.Catelogies.Countries
{
    public class DALCountry
    {
        ConnectFromServer Connect = new ConnectFromServer();

        public ObservableCollection<EntityCountry> GetCountry()
        {
            ObservableCollection<EntityCountry> data = new ObservableCollection<EntityCountry>();
            return Connect.GetDataReader<EntityCountry>("EXEC ViewOrSelectQuery 'TheWorld..Country'");
        }

        public void ActionCountry(EnumMethodApi type, EntityCountry value)
        {
            switch (type)
            {
                case EnumMethodApi.B:
                    Connect.ExecuteData($"EXEC InsertValueQuery 'TheWorld..Country', N'''{value.IsOnly}'',''{value.Code}'',''{value.NameEN}'',N''{value.NameVI}'',''{value.IsCurently}'',GETDATE(),''{value.CreateBy}'',GETDATE(),''{value.UpdateBy}'''");
                    break;
                case EnumMethodApi.C:
                    Connect.ExecuteData($"EXEC UpdateValueQuery 'TheWorld..Country',N'Code =''{value.Code}'', NameEN = ''{value.NameEN}'' , NameVI = N''{value.NameVI}'' , UpdateOn= GETDATE(), UpdateBy = ''{value.UpdateBy}''',N'IsOnly = ''{value.IsOnly}'''");
                    break;
                case EnumMethodApi.D:
                    Connect.ExecuteData($"EXEC DeleteQuery 'TheWorld..Country', N'IsOnly =''{value.IsOnly}'''");
                    break;
            }
        }
    }
}
