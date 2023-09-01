using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiNhanVien
{
    public class NhanVien
    {
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public string DienThoai { get; set; }
        public DateTime NgayVaoLam { get; set; }
        public double DoanhSo { get; set; }
        public double PCNhienLieu { get; set; }

        // Constructor
        public NhanVien()
        {
            // Khởi tạo các giá trị mặc định
            GioiTinh = "Nam";
            NgayVaoLam = DateTime.Now;
        }
    }
}
