using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLiNhanVien
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<NhanVien> danhSachNhanVien = new List<NhanVien>();
        private NhanVien nhanVienChon = null;
        public MainWindow()
        {
            InitializeComponent();
            radNam.IsChecked = true;
            dtpNgayVaoLam.SelectedDate = DateTime.Now;
            radBanHang.IsChecked = true;
            UpdateListView();
        }

        private void radBanHang_Checked(object sender, RoutedEventArgs e)
        {
            txtDoanhSo.Visibility = Visibility.Visible;
            txtPCNhienLieu.Visibility = Visibility.Collapsed;
            labelDoanhSo.Visibility = Visibility.Visible; // Show the label
            labelPCNhienLieu.Visibility = Visibility.Collapsed; // Hide the label
        }

        private void radGiaoNhan_Checked(object sender, RoutedEventArgs e)
        {
            txtDoanhSo.Visibility = Visibility.Collapsed;
            txtPCNhienLieu.Visibility = Visibility.Visible;
            labelDoanhSo.Visibility = Visibility.Collapsed; // Hide the label
            labelPCNhienLieu.Visibility = Visibility.Visible; // Show the label
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            ClearInputFields();
            txtMaNV.Focus();
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra thông tin hợp lệ
            if (string.IsNullOrEmpty(txtMaNV.Text) || string.IsNullOrEmpty(txtTen.Text))
            {
                MessageBox.Show("Vui lòng điền mã nhân viên và họ tên.");
                return;
            }

            if (dtpNgayVaoLam.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("Ngày vào làm không được lớn hơn ngày hiện tại.");
                return;
            }

            // Tạo đối tượng Nhân viên
            NhanVien nv = new NhanVien
            {
                MaNV = txtMaNV.Text,
                HoTen = txtTen.Text,
                GioiTinh = radNam.IsChecked == true ? "Nam" : "Nữ",
                DienThoai = txtDienThoai.Text,
                NgayVaoLam = dtpNgayVaoLam.SelectedDate.Value,
                DoanhSo = radBanHang.IsChecked == true ? Convert.ToDouble(txtDoanhSo.Text) : 0,
                PCNhienLieu = radGiaoNhan.IsChecked == true ? Convert.ToDouble(txtPCNhienLieu.Text) : 0
            };

            // Thêm vào danh sách và cập nhật ListView
            danhSachNhanVien.Add(nv);
            UpdateListView();
            ClearInputFields();
        }

        private void lvDanhSachNhanVien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDanhSachNhanVien.SelectedItem != null)
            {
                nhanVienChon = lvDanhSachNhanVien.SelectedItem as NhanVien;
                // Hiển thị thông tin Nhân viên lên các điều khiển nhập liệu
                txtMaNV.Text = nhanVienChon.MaNV;
                txtTen.Text = nhanVienChon.HoTen;
                txtDienThoai.Text = nhanVienChon.DienThoai;
                dtpNgayVaoLam.SelectedDate = nhanVienChon.NgayVaoLam;
                radNam.IsChecked = nhanVienChon.GioiTinh == "Nam";
                radNu.IsChecked = nhanVienChon.GioiTinh == "Nữ";
                if (radBanHang.IsChecked == true)
                    txtDoanhSo.Text = nhanVienChon.DoanhSo.ToString();
                else
                    txtPCNhienLieu.Text = nhanVienChon.PCNhienLieu.ToString();
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (nhanVienChon != null)
            {
                danhSachNhanVien.Remove(nhanVienChon);
                UpdateListView();
                ClearInputFields();
            }
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (nhanVienChon != null)
            {
                // Cập nhật thông tin của Nhân viên
                nhanVienChon.MaNV = txtMaNV.Text;
                nhanVienChon.HoTen = txtTen.Text;
                nhanVienChon.GioiTinh = radNam.IsChecked == true ? "Nam" : "Nữ";
                nhanVienChon.DienThoai = txtDienThoai.Text;
                nhanVienChon.NgayVaoLam = dtpNgayVaoLam.SelectedDate.Value;
                nhanVienChon.DoanhSo = radBanHang.IsChecked == true ? Convert.ToDouble(txtDoanhSo.Text) : 0;
                nhanVienChon.PCNhienLieu = radGiaoNhan.IsChecked == true ? Convert.ToDouble(txtPCNhienLieu.Text) : 0;

                UpdateListView();
            }
        }

        private void btnSapXep_Click(object sender, RoutedEventArgs e)
        {
            // Sắp xếp danh sách Nhân viên theo thâm niên giảm dần, nếu cùng thâm niên thì sắp xếp theo họ tên
            danhSachNhanVien = danhSachNhanVien.OrderByDescending(nv => DateTime.Now.Year - nv.NgayVaoLam.Year)
                                                 .ThenBy(nv => nv.HoTen)
                                                 .ToList();
            UpdateListView();
        }

        private void btnThongKe_Click(object sender, RoutedEventArgs e)
        {
            int soNhanVienBanHang = danhSachNhanVien.Count(nv => nv.DoanhSo > 0);
            int soNhanVienGiaoNhan = danhSachNhanVien.Count(nv => nv.PCNhienLieu > 0);
            double tongLuongBanHang = danhSachNhanVien.Where(nv => nv.DoanhSo > 0).Sum(nv => nv.DoanhSo);
            double tongLuongGiaoNhan = danhSachNhanVien.Where(nv => nv.PCNhienLieu > 0).Sum(nv => nv.PCNhienLieu);

            MessageBox.Show($"Số nhân viên bán hàng: {soNhanVienBanHang}\n" +
                            $"Số nhân viên giao nhận: {soNhanVienGiaoNhan}\n" +
                            $"Tổng lương bán hàng: {tongLuongBanHang}\n" +
                            $"Tổng lương giao nhận: {tongLuongGiaoNhan}");
        }

        private void UpdateListView()
        {
            lvDanhSachNhanVien.ItemsSource = null;
            lvDanhSachNhanVien.ItemsSource = danhSachNhanVien;
        }

        private void ClearInputFields()
        {
            txtMaNV.Clear();
            txtTen.Clear();
            txtDienThoai.Clear();
            dtpNgayVaoLam.SelectedDate = DateTime.Now;
            radNam.IsChecked = true;
            radNu.IsChecked = false;
            txtDoanhSo.Clear();
            txtPCNhienLieu.Clear();
            nhanVienChon = null;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng ứng dụng?", "Xác nhận đóng", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true; // Cancel the closing event if user selects "No"
            }
        }

    }
}
