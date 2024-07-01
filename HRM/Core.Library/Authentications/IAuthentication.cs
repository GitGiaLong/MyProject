using Core.Entities.Applications.Connects.API;
using Core.Entities.Managements.User;
using System.Runtime.CompilerServices;

namespace Core.Library.Authentications
{
    public interface IAuthentication
    {
        User User { get; set; }
        bool IsLogin { get; set; }
        Task Initialize();

        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
        Task Login();

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        Task Logout();
    }
}
