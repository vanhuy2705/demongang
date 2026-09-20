using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyThueSanTheThao.Data;

namespace QuanLyThueSanTheThao.Services;

public sealed class BookingService
{
    public DataTable GetFields() => Db.Query(@"
SELECT f.SanID,f.MaSan,f.TenSan,ft.TenLoaiSan,f.GiaMoiGio,f.TrangThai
FROM SanTheThao f INNER JOIN LoaiSan ft ON ft.LoaiSanID=f.LoaiSanID
WHERE f.DangHoatDong=1 ORDER BY ft.TenLoaiSan,f.TenSan;");

    public DataTable GetAvailableFields(DateTime start, DateTime end, int? fieldTypeId = null)
    {
        var sql = @"
SELECT f.SanID,f.MaSan,f.TenSan,ft.TenLoaiSan,f.GiaMoiGio
FROM SanTheThao f
INNER JOIN LoaiSan ft ON ft.LoaiSanID=f.LoaiSanID
WHERE f.DangHoatDong=1 AND f.TrangThai NOT IN (N'Bảo trì', N'Maintenance', N'Inactive', N'Ngừng hoạt động')
AND (@TypeID IS NULL OR f.LoaiSanID=@TypeID)
AND NOT EXISTS(
    SELECT 1 FROM DatSan b WITH (READCOMMITTEDLOCK)
    WHERE b.SanID=f.SanID
      AND b.TrangThai NOT IN ('Cancelled','Completed')
      AND @ThoiGianBatDau < b.ThoiGianKetThuc
      AND @ThoiGianKetThuc > b.ThoiGianBatDau
)
ORDER BY ft.TenLoaiSan,f.TenSan;";
        return Db.Query(sql,
            new SqlParameter("@TypeID", (object?)fieldTypeId ?? DBNull.Value),
            new SqlParameter("@ThoiGianBatDau", start),
            new SqlParameter("@ThoiGianKetThuc", end));
    }

    public bool IsAvailable(int fieldId, DateTime start, DateTime end, int? ignoreBookingId = null)
    {
        var count = Convert.ToInt32(Db.Scalar(@"
SELECT COUNT(*) FROM DatSan
WHERE SanID=@SanID
  AND TrangThai NOT IN ('Cancelled','Completed')
  AND (@IgnoreID IS NULL OR DatSanID<>@IgnoreID)
  AND @ThoiGianBatDau < ThoiGianKetThuc
  AND @ThoiGianKetThuc > ThoiGianBatDau;",
            new SqlParameter("@SanID", fieldId),
            new SqlParameter("@IgnoreID", (object?)ignoreBookingId ?? DBNull.Value),
            new SqlParameter("@ThoiGianBatDau", start),
            new SqlParameter("@ThoiGianKetThuc", end)) ?? 0);
        return count == 0;
    }

    public int CreateBooking(int customerId, int fieldId, DateTime start, DateTime end,
        int? voucherId, string note, int createdByUserId)
    {
        if (end <= start) throw new InvalidOperationException("Giờ kết thúc phải lớn hơn giờ bắt đầu.");

        using var cn = Db.OpenConnection();
        using var tx = cn.BeginTransaction(IsolationLevel.Serializable);
        try
        {
            using var check = new SqlCommand(@"
SELECT COUNT(*) FROM DatSan WITH (UPDLOCK,HOLDLOCK)
WHERE SanID=@SanID
  AND TrangThai NOT IN ('Cancelled','Completed')
  AND @ThoiGianBatDau < ThoiGianKetThuc AND @ThoiGianKetThuc > ThoiGianBatDau;", cn, tx);
            check.Parameters.AddWithValue("@SanID", fieldId);
            check.Parameters.AddWithValue("@ThoiGianBatDau", start);
            check.Parameters.AddWithValue("@ThoiGianKetThuc", end);
            if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                throw new InvalidOperationException("Khung giờ này vừa được người khác đặt. Vui lòng chọn thời gian khác.");

            decimal hourlyRate;
            using (var price = new SqlCommand("SELECT GiaMoiGio FROM SanTheThao WHERE SanID=@id AND DangHoatDong=1", cn, tx))
            {
                price.Parameters.AddWithValue("@id", fieldId);
                var raw = price.ExecuteScalar() ?? throw new InvalidOperationException("Sân không tồn tại hoặc đã ngừng hoạt động.");
                hourlyRate = Convert.ToDecimal(raw);
            }

            var hours = (decimal)(end - start).TotalHours;
            var subtotal = Math.Round(hourlyRate * hours, 0, MidpointRounding.AwayFromZero);
            decimal discount = 0;
            decimal voucherDiscountApplied = 0;
            int? promotionId = null;

            using (var promo = new SqlCommand(@"
SELECT TOP 1 KhuyenMaiID,PhanTramGiam
FROM KhuyenMai
WHERE DangHoatDong=1 AND SYSDATETIME() BETWEEN NgayBatDau AND NgayKetThuc
ORDER BY PhanTramGiam DESC, KhuyenMaiID DESC;", cn, tx))
            using (var pr = promo.ExecuteReader())
            {
                if (pr.Read())
                {
                    promotionId = Convert.ToInt32(pr["KhuyenMaiID"]);
                    discount += Math.Round(subtotal * Convert.ToDecimal(pr["PhanTramGiam"]) / 100m, 0, MidpointRounding.AwayFromZero);
                }
            }

            if (voucherId.HasValue)
            {
                using var vc = new SqlCommand(@"
SELECT TOP 1 v.LoaiGiamGia,v.GiaTriGiam,v.MucGiamToiDa
FROM PhieuGiamGia v
INNER JOIN PhieuGiamGiaKhachHang cv ON cv.PhieuGiamGiaID=v.PhieuGiamGiaID
WHERE cv.KhachHangID=@KhachHangID AND v.PhieuGiamGiaID=@PhieuGiamGiaID
AND cv.DaSuDung=0 AND v.DangHoatDong=1 AND SYSDATETIME() BETWEEN v.NgayBatDau AND v.NgayKetThuc
AND @TienGoc>=v.DonToiThieu;", cn, tx);
                vc.Parameters.AddWithValue("@KhachHangID", customerId);
                vc.Parameters.AddWithValue("@PhieuGiamGiaID", voucherId.Value);
                vc.Parameters.AddWithValue("@TienGoc", subtotal);
                using var r = vc.ExecuteReader();
                if (r.Read())
                {
                    var type = Convert.ToString(r["LoaiGiamGia"]) ?? "Fixed";
                    var value = Convert.ToDecimal(r["GiaTriGiam"]);
                    var max = r["MucGiamToiDa"] == DBNull.Value ? decimal.MaxValue : Convert.ToDecimal(r["MucGiamToiDa"]);
                    var voucherBase = Math.Max(0, subtotal - discount);
                    var voucherDiscount = type == "Percent" ? voucherBase * value / 100m : value;
                    voucherDiscount = Math.Min(voucherDiscount, max);
                    voucherDiscount = Math.Min(voucherDiscount, voucherBase);
                    voucherDiscountApplied = voucherDiscount;
                    discount += voucherDiscountApplied;
                }
            }
            var total = subtotal - discount;

            using var cmd = new SqlCommand(@"
INSERT INTO DatSan(MaDatSan,KhachHangID,SanID,ThoiGianBatDau,ThoiGianKetThuc,GiaMoiGio,TienGoc,TienGiam,TongTien,
 PhieuGiamGiaID,KhuyenMaiID,TrangThai,TrangThaiThanhToan,GhiChu,TaiKhoanTaoID,NgayTao)
OUTPUT INSERTED.DatSanID
VALUES(@TempCode,@KhachHangID,@SanID,@ThoiGianBatDau,@ThoiGianKetThuc,@GiaMoiGio,@TienGoc,@Discount,@Total,
 @PhieuGiamGiaID,@KhuyenMaiID,'Confirmed','Unpaid',@GhiChu,@CreatedBy,SYSDATETIME());", cn, tx);
            cmd.Parameters.AddWithValue("@TempCode", "TMP" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant());
            cmd.Parameters.AddWithValue("@KhachHangID", customerId);
            cmd.Parameters.AddWithValue("@SanID", fieldId);
            cmd.Parameters.AddWithValue("@ThoiGianBatDau", start);
            cmd.Parameters.AddWithValue("@ThoiGianKetThuc", end);
            cmd.Parameters.AddWithValue("@GiaMoiGio", hourlyRate);
            cmd.Parameters.AddWithValue("@TienGoc", subtotal);
            cmd.Parameters.AddWithValue("@Discount", discount);
            cmd.Parameters.AddWithValue("@Total", total);
            cmd.Parameters.AddWithValue("@PhieuGiamGiaID", (object?)voucherId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@KhuyenMaiID", (object?)promotionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GhiChu", note ?? "");
            cmd.Parameters.AddWithValue("@CreatedBy", createdByUserId);
            var bookingId = Convert.ToInt32(cmd.ExecuteScalar());

            var bookingCode = $"DS{bookingId:000000}";
            using var upd = new SqlCommand("UPDATE DatSan SET MaDatSan=@code WHERE DatSanID=@id", cn, tx);
            upd.Parameters.AddWithValue("@code", bookingCode);
            upd.Parameters.AddWithValue("@id", bookingId);
            upd.ExecuteNonQuery();

            using var invoice = new SqlCommand(@"
INSERT INTO HoaDon(MaHoaDon,DatSanID,KhachHangID,TienGoc,TienGiam,TongTien,SoTienDaTra,TrangThaiThanhToan,NgayTao)
VALUES(@Code,@DatSanID,@KhachHangID,@TienGoc,@Discount,@Total,0,'Unpaid',SYSDATETIME());", cn, tx);
            invoice.Parameters.AddWithValue("@Code", $"HD{bookingId:000000}");
            invoice.Parameters.AddWithValue("@DatSanID", bookingId);
            invoice.Parameters.AddWithValue("@KhachHangID", customerId);
            invoice.Parameters.AddWithValue("@TienGoc", subtotal);
            invoice.Parameters.AddWithValue("@Discount", discount);
            invoice.Parameters.AddWithValue("@Total", total);
            invoice.ExecuteNonQuery();

            if (voucherId.HasValue && voucherDiscountApplied > 0)
            {
                using var use = new SqlCommand(@"
UPDATE PhieuGiamGiaKhachHang SET DaSuDung=1,NgaySuDung=SYSDATETIME(),DatSanSuDungID=@DatSanID
WHERE KhachHangID=@KhachHangID AND PhieuGiamGiaID=@PhieuGiamGiaID AND DaSuDung=0;", cn, tx);
                use.Parameters.AddWithValue("@DatSanID", bookingId);
                use.Parameters.AddWithValue("@KhachHangID", customerId);
                use.Parameters.AddWithValue("@PhieuGiamGiaID", voucherId.Value);
                use.ExecuteNonQuery();
            }

            tx.Commit();
            return bookingId;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    public DataTable GetBookingsForUser(string role, int? customerId)
    {
        var where = role == "Customer" ? "WHERE b.KhachHangID=@KhachHangID" : "";
        return Db.Query($@"
SELECT b.DatSanID,b.MaDatSan,c.HoTen AS Customer,f.TenSan,ft.TenLoaiSan,
       b.ThoiGianBatDau,b.ThoiGianKetThuc,b.TongTien,b.TrangThai,b.TrangThaiThanhToan,b.NgayTao
FROM DatSan b
INNER JOIN KhachHang c ON c.KhachHangID=b.KhachHangID
INNER JOIN SanTheThao f ON f.SanID=b.SanID
INNER JOIN LoaiSan ft ON ft.LoaiSanID=f.LoaiSanID
{where}
ORDER BY b.ThoiGianBatDau DESC;",
            new SqlParameter("@KhachHangID", (object?)customerId ?? DBNull.Value));
    }
}
