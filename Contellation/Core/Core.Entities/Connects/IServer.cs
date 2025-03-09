using System.ComponentModel;

namespace Core.Entities.Connects
{
    public interface IServer
    {
        /// <summary>
        /// server name (Server name, Host name)
        /// </summary>
        string HostServerDB { get; set; }

        /// <summary>
        /// Port server name
        /// </summary>
        string PortServerDB { get; set; }

        /// <summary>
        /// Data base in server
        /// </summary>
        string DatabaseName { get; set; }

        [DisplayName("Tên Đăng Nhập")]
        string Username { get; set; }

        [DisplayName("Mật khẩu")]
        string Password { get; set; }
    }
}
