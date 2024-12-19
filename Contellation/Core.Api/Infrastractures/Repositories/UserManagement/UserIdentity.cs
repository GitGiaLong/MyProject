using Core.Api.Connect;
using Core.Entities.UserManagement;
using Core.Libraries.Connects;
using Core.Libraries.Extensions;
using Core.Libraries.Models;

namespace Core.Api.Infrastractures.Repositories.UserManagement
{
    public class UserIdentity : IUserIdentity
    {
        private readonly IConnectServers connect = new ConnectServers();
        //public UserIdentity(IConnectServers? _connect)
        //{
        //    connect = _connect;
        //}

        public IFilter code { get; set; } = new Filter();
        public Account Identity(Account en)
        {
            code.Value = "Where Username= '" + en.Username.Trim() + "' and Password='" + en.Password.Trim() + "'" ?? "";
            //var a = GetDataReader<Account>($"{SelectQuery(code, EDatabase.UserManagement.ToEnumString(), "AccountUser")}");
            return connect.GetDataReader<Account>($"{connect.SelectQuery(code, EDatabase.UserManagement.ToEnumString(), "AccountUser")}").First();
        }
    }
}
