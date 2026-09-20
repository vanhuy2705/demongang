using Microsoft.Data.SqlClient;
using QuanLyThueSanTheThao.Data;
using QuanLyThueSanTheThao.Helpers;
using QuanLyThueSanTheThao.Models;

namespace QuanLyThueSanTheThao.Services;

public sealed class AuthService
{
    public LoginResult Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return new LoginResult { Success = false, Message = "Vui lòng nhập tài khoản và mật khẩu." };

        const string sql = @"
SELECT TOP 1 u.TaiKhoanID,u.TenDangNhap,u.MatKhauBam,u.MuoiMatKhau,u.DangHoatDong,
       r.TenVaiTro,
       c.KhachHangID,
       e.NhanVienID,
       COALESCE(c.HoTen,e.HoTen,u.TenDangNhap) AS HoTen
FROM TaiKhoan u
INNER JOIN VaiTro r ON r.VaiTroID=u.VaiTroID
LEFT JOIN KhachHang c ON c.TaiKhoanID=u.TaiKhoanID
LEFT JOIN NhanVien e ON e.TaiKhoanID=u.TaiKhoanID
WHERE u.TenDangNhap=@TenDangNhap;";

        var dt = Db.Query(sql, new SqlParameter("@TenDangNhap", username.Trim()));
        if (dt.Rows.Count == 0)
            return new LoginResult { Success = false, Message = "Tài khoản không tồn tại." };

        var row = dt.Rows[0];
        if (!Convert.ToBoolean(row["DangHoatDong"]))
            return new LoginResult { Success = false, Message = "Tài khoản đang bị khóa." };

        var salt = Convert.ToString(row["MuoiMatKhau"]) ?? "";
        var expected = Convert.ToString(row["MatKhauBam"]) ?? "";
        if (!string.Equals(expected, PasswordHelper.Hash(salt, password), StringComparison.OrdinalIgnoreCase))
            return new LoginResult { Success = false, Message = "Mật khẩu không đúng." };

        return new LoginResult
        {
            Success = true,
            UserId = Convert.ToInt32(row["TaiKhoanID"]),
            Username = Convert.ToString(row["TenDangNhap"]) ?? "",
            Role = Convert.ToString(row["TenVaiTro"]) ?? "",
            FullName = Convert.ToString(row["HoTen"]) ?? username,
            CustomerId = row["KhachHangID"] == DBNull.Value ? null : Convert.ToInt32(row["KhachHangID"]),
            EmployeeId = row["NhanVienID"] == DBNull.Value ? null : Convert.ToInt32(row["NhanVienID"])
        };
    }
}
