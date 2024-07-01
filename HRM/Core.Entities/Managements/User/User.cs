using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Managements.User
{
    public class User
    {
        ///// <summary>
        ///// Id bảng employee
        ///// </summary>
        //[DisplayName("Mã Nhân Viên")]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string IsOnly { get; set; }

        //UserName
        [DisplayName("Tên Đăng Nhập")]
        public string Username { get; set; }

        [DisplayName("Mật khẩu")]
        public string Password { get; set; }

        /// <summary>
        /// Mức độ Level
        /// </summary>
        //public int AccessLevel { get; set; }
        //public bool ReadOnly { get; set; } = false;

        /// <summary>
        /// Danh sách ứng dụng được phép truy cập
        /// </summary>
        //public List<string> ListKeyApp { get; set; }
        /// <summary>
        /// Nhân viên đang chọn làm việc ở cty nào, có thể thay đổi khi chọn lại công ty
        /// Lưu ý muốn lấy danh sách nhân viên theo công ty thì vào bảng company
        /// </summary>
        //public List<string> ListCompanyId { get; set; }

        /// <summary>
        /// Id của thiết bị, dùng để gửi nhận notification
        /// </summary>
        //[Display(Name = "Thiết bị")]
        //public List<string> DeviceId { get; set; }

        //public string Note { get; set; }
    }
}
