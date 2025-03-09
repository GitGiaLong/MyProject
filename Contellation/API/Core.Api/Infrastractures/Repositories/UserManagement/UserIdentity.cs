using Core.Api.Connects;
using Core.Api.JwtSetup;
using Core.Entities.Connects;
using Core.Entities.UserManagement;
using Core.Libraries.Connects;
using Core.Libraries.Extensions;
using Core.Libraries.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Reflection;

namespace Core.Api.Infrastractures.Repositories.UserManagement
{
    public class UserIdentity : IUserIdentity
    {
        private readonly IConnectServers connect = new ConnectServers();

        //private readonly IBuildJwt _jwtProvider;
        //public UserIdentity(IBuildJwt jwtHelper)
        //{
        //    _jwtProvider = jwtHelper;
        //}

        public IFilter code { get; set; } = new Filter();
        //public Account Identity(Account en)
        //{
        //    code.Value = "Where Username= '" + en.Username.Trim() + "' and Password='" + en.Password.Trim() + "'" ?? "";
        //    //var a = GetDataReader<Account>($"{SelectQuery(code, EDatabase.UserManagement.ToEnumString(), "AccountUser")}");
        //    return connect.GetDataReader<Account>($"{connect.SelectQuery(code, EDatabase.UserManagement.ToEnumString(), "AccountUser")}").First();
        //}

        public object Identity(object ens, IBuildJwt _jwtProvider)
        {
            
             Entities.Connects.Account en = (Entities.Connects.Account)ens;
            //if (!en.Username.IsNullOrEmpty() && !en.Password.IsNullOrEmpty())
            //{
            //    code.Value = "Where Username= '" + en.Username.Trim() + "' and Password='" + en.Password.Trim() + "'" ?? "";

            //    connect.GetDataReader<IAccount>($"{connect.SelectQuery(code, EDatabase.UserManagement.ToEnumString(), "AccountUser")}").First();
            //    //string token = _jwtProvider.GenerateToken("");
            //    //if (!token.IsNullOrEmpty())
            //    //{
            //    //    return Ok(new { token });
            //    //}
            //    //else
            //    //{
            //    //    return NotFound();
            //    //}
            //}
            //var a = GetDataReader<Account>($"{SelectQuery(code, EDatabase.UserManagement.ToEnumString(), "AccountUser")}");


            return "";
            //return connect.GetDataReader<Account>($"{connect.SelectQuery(code, EDatabase.UserManagement.ToEnumString(), "AccountUser")}").First();
        }

        string Check()
        {
            IFilter code = new Filter();
            //code.Value = "Where Username= '" + en.Username.Trim() + "' and Password='" + en.Password.Trim() + "'" ?? "";


            return "";
            //return connect.GetDataReader<Account>($"{connect.SelectQuery(code, EDatabase.UserManagement.ToEnumString(), "AccountUser")}").First();
        }
    }
}
