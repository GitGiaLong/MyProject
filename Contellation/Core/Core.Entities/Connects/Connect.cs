namespace Core.Entities.Connects
{
    public class Connect : Server, IAccount, IAPI, IServer
    {


        /* Login */

        ///// <summary>
        ///// Id bảng employee
        ///// </summary>
        //[DisplayName("Mã Nhân Viên")]
        //public string IsOnly { get; set; }


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

        // Connect API

        private string _Url { get; set; } = "https://localhost:44388/";
        public string Url { get { return _Url; } set { _Url = value; } }

        private string _Token { get; set; } = string.Empty;
        public string Token { get { return _Token; } set { _Token = value; } }

        private double _TimeOut { get; set; } = 5;
        public double TimeOut { get { return _TimeOut; } set { _TimeOut = value; } }
    }
}
