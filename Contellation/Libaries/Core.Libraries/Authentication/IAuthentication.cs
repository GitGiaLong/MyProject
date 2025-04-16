using Core.Entities.Connects;

namespace Core.Libraries.Authentication
{
    public interface IAuthentication
    {
        Account User { get; set; }
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
