using Core.Entities.Applications.Base.Filters;
using Core.Entities.Applications.Connects.Enums;
using Core.Entities.Managements.User;
using Core.Library.Applications.Server;
using Core.Library.Extensions;

namespace Core.API.Infrastracture.Repositories.UserManagement
{
    public class UserIdentity : IUserIdentity
    {
        private readonly IConnectServers connect;

        public UserIdentity(IConnectServers _connect)
        {
            connect = _connect;
        }
        public Filter code = new Filter();
        public User Identity(User en)
        {
            code.Value = "Where Username= N'" + en.Username.Trim() + "' and Password='" + en.Password.Trim() + "'" ?? "";
            //var a = GetDataReader<User>($"{SelectQuery(code, EDatabase.UserManagement.ToEnumString(), "AccountUser")}");
            return connect.GetDataReader<User>($"{connect.SelectQuery(code, EDatabase.UserManagement.ToEnumString(), "AccountUser")}").First();
        }
    }
}
