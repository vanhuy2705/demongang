using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyThueSanTheThao.Data;

namespace QuanLyThueSanTheThao.Services;

public sealed class PaymentService
{
    public DataRow GetPaymentInfo(int bookingId)
    {
        var dt = Db.Query(@"
SELECT b.DatSanID,b.MaDatSan,b.TongTien,b.TrangThaiThanhToan,
       i.HoaDonID,i.MaHoaDon,i.SoTienDaTra,
       c.HoTen AS CustomerName,f.TenSan,b.ThoiGianBatDau,b.ThoiGianKetThuc
FROM DatSan b
INNER JOIN HoaDon i ON i.DatSanID=b.DatSanID
INNER JOIN KhachHang c ON c.KhachHangID=b.KhachHangID
INNER JOIN SanTheThao f ON f.SanID=b.SanID
WHERE b.DatSanID=@id;", new SqlParameter("@id", bookingId));
        if (dt.Rows.Count == 0) throw new InvalidOperationException("Không tìm thấy đơn đặt sân.");
        return dt.Rows[0];
    }

    public void RecordPayment(int invoiceId, decimal amount, string method, string transactionRef, bool confirmPaid)
    {
        using var cn = Db.OpenConnection();
        using var tx = cn.BeginTransaction();
        try
        {
            using var cmd = new SqlCommand(@"
INSERT INTO ThanhToan(HoaDonID,SoTien,PhuongThucThanhToan,MaGiaoDich,TrangThai,NgayThanhToan,NgayTao)
VALUES(@HoaDonID,@SoTien,@Method,@Ref,@TrangThai,CASE WHEN @TrangThai='Confirmed' THEN SYSDATETIME() ELSE NULL END,SYSDATETIME());", cn, tx);
            cmd.Parameters.AddWithValue("@HoaDonID", invoiceId);
            cmd.Parameters.AddWithValue("@SoTien", amount);
            cmd.Parameters.AddWithValue("@Method", method);
            cmd.Parameters.AddWithValue("@Ref", transactionRef ?? "");
            cmd.Parameters.AddWithValue("@TrangThai", confirmPaid ? "Confirmed" : "Pending");
            cmd.ExecuteNonQuery();

            if (confirmPaid)
            {
                using var upd = new SqlCommand(@"
UPDATE HoaDon SET SoTienDaTra=SoTienDaTra+@SoTien,
TrangThaiThanhToan=CASE WHEN SoTienDaTra+@SoTien>=TongTien THEN 'Paid' ELSE 'PartiallyPaid' END
WHERE HoaDonID=@HoaDonID;
UPDATE b SET TrangThaiThanhToan=i.TrangThaiThanhToan
FROM DatSan b INNER JOIN HoaDon i ON i.DatSanID=b.DatSanID
WHERE i.HoaDonID=@HoaDonID;", cn, tx);
                upd.Parameters.AddWithValue("@SoTien", amount);
                upd.Parameters.AddWithValue("@HoaDonID", invoiceId);
                upd.ExecuteNonQuery();
            }
            tx.Commit();
        }
        catch { tx.Rollback(); throw; }
    }
}
