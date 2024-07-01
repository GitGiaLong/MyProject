using System.Runtime.Serialization;

namespace Core.Entities.Applications.Connects.API
{
    public enum EMethodApi
    {
        /// <summary>
        /// Sử dụng để lấy thông tin từ server theo URI đã cung cấp.
        /// </summary>
        [EnumMember(Value = "GET")] Get,

        /// <summary>
        /// Giống với GET nhưng response trả về không có body, chỉ có header
        /// </summary>
        [EnumMember(Value = "HEAD")] Head,
        /// <summary>
        /// Gửi thông tin tới sever thông qua các parameters HTTP.
        /// </summary>
        [EnumMember(Value = "POST")] Post,

        /// <summary>
        /// Ghi đè tất cả thông tin của đối tượng với những gì được gửi lên.
        /// </summary>
        [EnumMember(Value = "PUT")] Put,

        /// <summary>
        /// Ghi đè các thông tin được thay đổi của đối tượng.
        /// </summary>
        [EnumMember(Value = "PATCH")] Patch,

        /// <summary>
        /// Xóa resource trên server.
        /// </summary>
        [EnumMember(Value = "DELETE")] Delete,

        /// <summary>
        /// Thiết lập một kết nối tới server theo URI.
        /// </summary>
        [EnumMember(Value = "CONNECT")] Connect,

        /// <summary>
        /// Mô tả các tùy chọn giao tiếp cho resource.
        /// </summary>
        [EnumMember(Value = "OPTIONS")] Options,

        /// <summary>
        /// Thực hiện một bài test loop-back theo đường dẫn đến resource.
        /// </summary>
        [EnumMember(Value = "TRACE")] Trace
    }
}
