using System.ComponentModel.DataAnnotations;

namespace Core.Entities.UserManagement
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        public string Role { get; set; } // Guest, User, Friend, Family, Admin
        public string ProfileData { get; set; } // JSON cho thần số học, chiêm tinh
        public string Resume { get; set; } // Hồ sơ tuyển dụng
    }
}
