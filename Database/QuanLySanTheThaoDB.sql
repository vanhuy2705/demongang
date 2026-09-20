/* ============================================================
   QUANLYSANTHETHAODB — Script tạo CSDL đầy đủ
   Máy chủ : localhost\SQLEXPRESS (SQL Server 2022 Express)
   File MDF/LDF tạo tại:
   C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA

   Cách chạy (chọn 1 trong 2):
   1) SSMS: mở file này -> Execute (F5)
   2) CMD:  sqlcmd -S localhost\SQLEXPRESS -E -i "đường-dẫn\QuanLySanTheThaoDB.sql"

   Tài khoản demo (mật khẩu đã băm SHA256 theo PasswordHelper.cs):
     admin    / admin123     (Quản trị)
     staff    / staff123     (Nhân viên)
     customer / customer123  (Khách hàng)
   ============================================================ */

USE master;
GO

IF DB_ID(N'QuanLySanTheThaoDB') IS NULL
BEGIN
    CREATE DATABASE [QuanLySanTheThaoDB]
    ON PRIMARY
    ( NAME = N'QuanLySanTheThaoDB_Data',
      FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\QuanLySanTheThaoDB.mdf',
      SIZE = 60MB, FILEGROWTH = 20MB )
    LOG ON
    ( NAME = N'QuanLySanTheThaoDB_Log',
      FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\QuanLySanTheThaoDB_log.ldf',
      SIZE = 30MB, FILEGROWTH = 20MB );
END
GO

ALTER DATABASE [QuanLySanTheThaoDB] SET AUTO_CLOSE OFF;
GO

USE [QuanLySanTheThaoDB];
GO

/* ===================== BẢNG ===================== */

IF OBJECT_ID(N'dbo.VaiTro', N'U') IS NULL
CREATE TABLE dbo.VaiTro
(
    VaiTroID     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TenVaiTro    NVARCHAR(30)  NOT NULL UNIQUE,   -- Admin | Employee | Customer
    TenHienThi   NVARCHAR(100) NOT NULL
);
GO

IF OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NULL
CREATE TABLE dbo.TaiKhoan
(
    TaiKhoanID     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TenDangNhap    NVARCHAR(50)  NOT NULL UNIQUE,
    MatKhauBam     VARCHAR(64)   NOT NULL,          -- SHA256(salt + matkhau) hex thường
    MuoiMatKhau    VARCHAR(20)   NOT NULL,
    VaiTroID       INT           NOT NULL FOREIGN KEY REFERENCES dbo.VaiTro(VaiTroID),
    DangHoatDong   BIT           NOT NULL DEFAULT 1,
    DangNhapLanCuoi DATETIME2    NULL,
    NgayTao        DATETIME2     NOT NULL DEFAULT SYSDATETIME()
);
GO

IF OBJECT_ID(N'dbo.KhachHang', N'U') IS NULL
CREATE TABLE dbo.KhachHang
(
    KhachHangID  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MaKhachHang  NVARCHAR(20)  NOT NULL UNIQUE,
    TaiKhoanID   INT           NULL UNIQUE FOREIGN KEY REFERENCES dbo.TaiKhoan(TaiKhoanID),
    HoTen        NVARCHAR(100) NOT NULL,
    SoDienThoai  NVARCHAR(20)  NULL,
    Email        NVARCHAR(100) NULL,
    DiaChi       NVARCHAR(200) NULL,
    NgaySinh     DATE          NULL,
    DiemTichLuy  INT           NOT NULL DEFAULT 0,
    DangHoatDong BIT           NOT NULL DEFAULT 1,
    NgayTao      DATETIME2     NOT NULL DEFAULT SYSDATETIME()
);
GO

IF OBJECT_ID(N'dbo.NhanVien', N'U') IS NULL
CREATE TABLE dbo.NhanVien
(
    NhanVienID   INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MaNhanVien   NVARCHAR(20)  NOT NULL UNIQUE,
    TaiKhoanID   INT           NULL UNIQUE FOREIGN KEY REFERENCES dbo.TaiKhoan(TaiKhoanID),
    HoTen        NVARCHAR(100) NOT NULL,
    SoDienThoai  NVARCHAR(20)  NULL,
    Email        NVARCHAR(100) NULL,
    ChucVu       NVARCHAR(60)  NULL,
    DangHoatDong BIT           NOT NULL DEFAULT 1
);
GO

IF OBJECT_ID(N'dbo.LoaiSan', N'U') IS NULL
CREATE TABLE dbo.LoaiSan
(
    LoaiSanID   INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MaLoaiSan   NVARCHAR(20)  NOT NULL UNIQUE,
    TenLoaiSan  NVARCHAR(100) NOT NULL,
    MoTa        NVARCHAR(300) NULL,
    DangHoatDong BIT          NOT NULL DEFAULT 1
);
GO

IF OBJECT_ID(N'dbo.SanTheThao', N'U') IS NULL
CREATE TABLE dbo.SanTheThao
(
    SanID       INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MaSan       NVARCHAR(20)  NOT NULL UNIQUE,
    TenSan      NVARCHAR(100) NOT NULL,
    LoaiSanID   INT           NOT NULL FOREIGN KEY REFERENCES dbo.LoaiSan(LoaiSanID),
    ViTri       NVARCHAR(100) NULL,
    GiaMoiGio   DECIMAL(12,2) NOT NULL,
    TrangThai   NVARCHAR(30)  NOT NULL DEFAULT N'Available',  -- Available | Busy | Maintenance | Inactive | N'Trống'... (tương thích)
    MoTa        NVARCHAR(300) NULL,
    DangHoatDong BIT          NOT NULL DEFAULT 1
);
GO

IF OBJECT_ID(N'dbo.KhuyenMai', N'U') IS NULL
CREATE TABLE dbo.KhuyenMai
(
    KhuyenMaiID  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MaKhuyenMai  NVARCHAR(20)  NOT NULL UNIQUE,
    TenKhuyenMai NVARCHAR(150) NOT NULL,
    MoTa         NVARCHAR(300) NULL,
    PhanTramGiam DECIMAL(5,2)  NOT NULL,
    NgayBatDau   DATETIME2     NOT NULL,
    NgayKetThuc  DATETIME2     NOT NULL,
    DangHoatDong BIT           NOT NULL DEFAULT 1
);
GO

IF OBJECT_ID(N'dbo.PhieuGiamGia', N'U') IS NULL
CREATE TABLE dbo.PhieuGiamGia
(
    PhieuGiamGiaID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MaPhieuGiamGia NVARCHAR(20)  NOT NULL UNIQUE,
    TenPhieuGiamGia NVARCHAR(150) NOT NULL,
    LoaiGiamGia    NVARCHAR(10)  NOT NULL CHECK (LoaiGiamGia IN (N'Percent', N'Fixed')),
    GiaTriGiam     DECIMAL(14,2) NOT NULL,
    MucGiamToiDa   DECIMAL(14,2) NULL,
    DonToiThieu    DECIMAL(14,2) NOT NULL DEFAULT 0,
    NgayBatDau     DATETIME2     NOT NULL,
    NgayKetThuc    DATETIME2     NOT NULL,
    SoLuong        INT           NULL,
    DangHoatDong   BIT           NOT NULL DEFAULT 1
);
GO

IF OBJECT_ID(N'dbo.DatSan', N'U') IS NULL
CREATE TABLE dbo.DatSan
(
    DatSanID            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MaDatSan            NVARCHAR(20)  NOT NULL UNIQUE,
    KhachHangID         INT           NOT NULL FOREIGN KEY REFERENCES dbo.KhachHang(KhachHangID),
    SanID               INT           NOT NULL FOREIGN KEY REFERENCES dbo.SanTheThao(SanID),
    ThoiGianBatDau      DATETIME2     NOT NULL,
    ThoiGianKetThuc     DATETIME2     NOT NULL,
    GiaMoiGio           DECIMAL(12,2) NOT NULL DEFAULT 0,
    TienGoc             DECIMAL(14,2) NOT NULL DEFAULT 0,
    TienGiam            DECIMAL(14,2) NOT NULL DEFAULT 0,
    TongTien            DECIMAL(14,2) NOT NULL DEFAULT 0,
    PhieuGiamGiaID      INT           NULL FOREIGN KEY REFERENCES dbo.PhieuGiamGia(PhieuGiamGiaID),
    KhuyenMaiID         INT           NULL FOREIGN KEY REFERENCES dbo.KhuyenMai(KhuyenMaiID),
    TrangThai           NVARCHAR(20)  NOT NULL DEFAULT N'Pending',   -- Pending|Confirmed|InUse|Completed|Cancelled
    TrangThaiThanhToan  NVARCHAR(20)  NOT NULL DEFAULT N'Unpaid',    -- Unpaid|PartiallyPaid|Paid
    GhiChu              NVARCHAR(300) NULL,
    TaiKhoanTaoID       INT           NULL FOREIGN KEY REFERENCES dbo.TaiKhoan(TaiKhoanID),
    NgayTao             DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    NgayCapNhat         DATETIME2     NULL
);
GO

IF OBJECT_ID(N'dbo.PhieuGiamGiaKhachHang', N'U') IS NULL
CREATE TABLE dbo.PhieuGiamGiaKhachHang
(
    PhieuGiamGiaKhachHangID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    PhieuGiamGiaID INT NOT NULL FOREIGN KEY REFERENCES dbo.PhieuGiamGia(PhieuGiamGiaID),
    KhachHangID    INT NOT NULL FOREIGN KEY REFERENCES dbo.KhachHang(KhachHangID),
    NgayNhan       DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    DaSuDung       BIT       NOT NULL DEFAULT 0,
    NgaySuDung     DATETIME2 NULL,
    DatSanSuDungID INT       NULL FOREIGN KEY REFERENCES dbo.DatSan(DatSanID),
    CONSTRAINT UQ_PGGKH UNIQUE (PhieuGiamGiaID, KhachHangID)
);
GO

IF OBJECT_ID(N'dbo.HoaDon', N'U') IS NULL
CREATE TABLE dbo.HoaDon
(
    HoaDonID           INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MaHoaDon           NVARCHAR(20)  NOT NULL UNIQUE,
    DatSanID           INT           NOT NULL UNIQUE FOREIGN KEY REFERENCES dbo.DatSan(DatSanID),
    KhachHangID        INT           NOT NULL FOREIGN KEY REFERENCES dbo.KhachHang(KhachHangID),
    TienGoc            DECIMAL(14,2) NOT NULL DEFAULT 0,
    TienGiam           DECIMAL(14,2) NOT NULL DEFAULT 0,
    TongTien           DECIMAL(14,2) NOT NULL DEFAULT 0,
    SoTienDaTra        DECIMAL(14,2) NOT NULL DEFAULT 0,
    TrangThaiThanhToan NVARCHAR(20)  NOT NULL DEFAULT N'Unpaid',
    NgayTao            DATETIME2     NOT NULL DEFAULT SYSDATETIME()
);
GO

IF OBJECT_ID(N'dbo.ThanhToan', N'U') IS NULL
CREATE TABLE dbo.ThanhToan
(
    ThanhToanID        INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    HoaDonID           INT           NOT NULL FOREIGN KEY REFERENCES dbo.HoaDon(HoaDonID),
    SoTien             DECIMAL(14,2) NOT NULL,
    PhuongThucThanhToan NVARCHAR(20) NOT NULL,   -- Cash | QR | BankTransfer
    MaGiaoDich         NVARCHAR(100) NULL,
    TrangThai          NVARCHAR(20)  NOT NULL DEFAULT N'Pending',  -- Pending | Confirmed
    NgayThanhToan      DATETIME2     NULL,
    NgayTao            DATETIME2     NOT NULL DEFAULT SYSDATETIME()
);
GO

IF OBJECT_ID(N'dbo.CauHinhHeThong', N'U') IS NULL
CREATE TABLE dbo.CauHinhHeThong
(
    KhoaCauHinh  NVARCHAR(50)  NOT NULL PRIMARY KEY,
    GiaTriCauHinh NVARCHAR(300) NOT NULL,
    NgayCapNhat  DATETIME2     NOT NULL DEFAULT SYSDATETIME()
);
GO

/* ===================== CHỈ MỤC ===================== */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_DatSan_San_ThoiGian')
    CREATE INDEX IX_DatSan_San_ThoiGian ON dbo.DatSan(SanID, ThoiGianBatDau, ThoiGianKetThuc);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_DatSan_KhachHang')
    CREATE INDEX IX_DatSan_KhachHang ON dbo.DatSan(KhachHangID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_ThanhToan_HoaDon')
    CREATE INDEX IX_ThanhToan_HoaDon ON dbo.ThanhToan(HoaDonID, TrangThai);
GO

/* ===================== DỮ LIỆU MẶC ĐỊNH ===================== */

IF NOT EXISTS (SELECT 1 FROM dbo.VaiTro)
INSERT INTO dbo.VaiTro(TenVaiTro, TenHienThi) VALUES
(N'Admin',    N'Quản trị viên'),
(N'Employee', N'Nhân viên'),
(N'Customer', N'Khách hàng');
GO

-- Mật khẩu: SHA256(salt + matkhau) — khớp Helpers/PasswordHelper.cs
IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan)
INSERT INTO dbo.TaiKhoan(TenDangNhap, MatKhauBam, MuoiMatKhau, VaiTroID, DangHoatDong) VALUES
(N'admin',    '41fc5551ad85815175b1680b4d5f75ce0680dd001af44c201ec537a8cb4324f6', '9F3C81EA', 1, 1),  -- admin123
(N'staff',    '9a44e76a10bb39c0c012a029b06103768e44461149b3c67af9762b126f008ce1', '5D7E24B6', 2, 1),  -- staff123
(N'customer', '2fac7e3589e41edcecba65961cf4a798920181f319fee2eba0e5ee6d11792fce', 'C8E41A72', 3, 1); -- customer123
GO

IF NOT EXISTS (SELECT 1 FROM dbo.NhanVien)
INSERT INTO dbo.NhanVien(MaNhanVien, TaiKhoanID, HoTen, SoDienThoai, Email, ChucVu) VALUES
(N'NV001', 2, N'Trần Minh Staff', N'0987 654 321', N'staff@sportfield.vn', N'Quầy lễ tân');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.KhachHang)
INSERT INTO dbo.KhachHang(MaKhachHang, TaiKhoanID, HoTen, SoDienThoai, Email, DiaChi, NgaySinh, DiemTichLuy) VALUES
(N'KH001', 3, N'Nguyễn Văn An',    N'0901 234 567', N'an.nguyen@gmail.com',    N'12 Nguyễn Trãi, Thanh Xuân, Hà Nội', '1990-03-12', 120),
(N'KH002', NULL, N'Trần Thị Bích', N'0912 345 678', N'bich.tran@outlook.com',  N'45 Giải Phóng, Hai Bà Trưng, Hà Nội', '1995-06-01', 60),
(N'KH003', NULL, N'Lê Minh Tuấn',  N'0934 567 890', N'tuan.le@yahoo.com',      N'78 Cầu Giấy, Hà Nội', '1988-11-22', 35);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.LoaiSan)
INSERT INTO dbo.LoaiSan(MaLoaiSan, TenLoaiSan, MoTa) VALUES
(N'LS01', N'Sân bóng đá 5 người',   N'Sân 5 tiêu chuẩn, cỏ nhân tạo'),
(N'LS02', N'Sân bóng đá 7 người',   N'Sân 7 có đèn chiếu sáng'),
(N'LS03', N'Sân bóng đá 11 người',  N'Sân 11 chuẩn thi đấu'),
(N'LS04', N'Sân cầu lông',          N'Nội thất chuẩn thi đấu'),
(N'LS05', N'Sân bóng chuyền',       N'Sân trong nhà, thoáng mát');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.SanTheThao)
INSERT INTO dbo.SanTheThao(MaSan, TenSan, LoaiSanID, ViTri, GiaMoiGio, TrangThai, MoTa) VALUES
(N'SA1',  N'Sân A1 (5 người)',    1, N'Tầng 1 - Khối A', 120000, N'Available', N'Cỏ nhân tạo mới'),
(N'SA2',  N'Sân A2 (5 người)',    1, N'Tầng 1 - Khối A', 120000, N'Available', NULL),
(N'SB1',  N'Sân B1 (7 người)',    2, N'Khối B',          200000, N'Available', N'Đèn LED 2 bên'),
(N'SC1',  N'Sân C1 (11 người)',   3, N'Sân chính',       350000, N'Available', N'Chuẩn thi đấu cấp quận'),
(N'SCL1', N'Sân cầu lông 01',     4, N'Tầng 2 - Khối C',  80000, N'Available', NULL),
(N'SCL2', N'Sân cầu lông 02',     4, N'Tầng 2 - Khối C',  80000, N'Available', NULL),
(N'SBC1', N'Sân bóng chuyền 01',  5, N'Nhà thi đấu',     100000, N'Available', NULL);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.KhuyenMai)
INSERT INTO dbo.KhuyenMai(MaKhuyenMai, TenKhuyenMai, MoTa, PhanTramGiam, NgayBatDau, NgayKetThuc, DangHoatDong) VALUES
(N'KM0926', N'Khuyến mãi đầu tháng 9', N'Giảm 10% mọi lượt đặt trong tháng 9', 10, '2026-09-01 00:00', '2026-09-30 23:59', 1);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.PhieuGiamGia)
INSERT INTO dbo.PhieuGiamGia(MaPhieuGiamGia, TenPhieuGiamGia, LoaiGiamGia, GiaTriGiam, MucGiamToiDa, DonToiThieu, NgayBatDau, NgayKetThuc, SoLuong, DangHoatDong) VALUES
(N'VOUCHER20', N'Voucher thành viên — giảm 20%', N'Percent', 20, 100000, 100000, '2026-09-01 00:00', '2026-12-31 23:59', 100, 1),
(N'SAVE500',   N'Giảm 500K đơn từ 2 triệu',      N'Fixed',  500000, NULL, 2000000, '2026-09-01 00:00', '2026-12-31 23:59', 50, 1);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.PhieuGiamGiaKhachHang)
INSERT INTO dbo.PhieuGiamGiaKhachHang(PhieuGiamGiaID, KhachHangID, DaSuDung) VALUES
(1, 1, 0);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.CauHinhHeThong)
INSERT INTO dbo.CauHinhHeThong(KhoaCauHinh, GiaTriCauHinh) VALUES
(N'CompanyName',       N'SportField — Thuê sân thể thao'),
(N'BankName',          N'TECHCOMBANK'),
(N'BankBin',           N'970407'),
(N'BankAccountNumber', N'190366528888'),
(N'BankAccountName',   N'NGUYEN VAN AN'),
(N'QrTransferPrefix',  N'THANHTOAN'),
(N'OpenTime',          N'05:00'),
(N'CloseTime',         N'23:00');
GO

PRINT N'✅ CSDL QuanLySanTheThaoDB đã sẵn sàng. Đăng nhập thử: admin / admin123';
GO
