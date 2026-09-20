CSDL QUANLYSANTHETHAODB — HƯỚNG DẪN NHANH
============================================
File: QuanLySanTheThaoDB.sql (tạo DB + bảng + dữ liệu mẫu)
Instance: localhost\SQLEXPRESS
File MDF/LDF tạo tại:
  C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA

CÁCH CHẠY (chọn 1 trong 2)
1) SSMS: đăng nhập localhost\SQLEXPRESS -> File > Open > File...
   chọn QuanLySanTheThaoDB.sql -> Execute (F5)
2) CMD:
   sqlcmd -S localhost\SQLEXPRESS -E -i "%cd%\QuanLySanTheThaoDB.sql"

SAU ĐÓ: chạy ứng dụng (dotnet run hoặc F5 trong Visual Studio).
Kết nối đã cấu hình sẵn trong Data\DatabaseConfig.cs:
  Server=localhost\SQLEXPRESS;Database=QuanLySanTheThaoDB;
  Trusted_Connection=True;TrustServerCertificate=True;

TÀI KHOẢN DEMO
  admin    / admin123     (Quản trị viên)
  staff    / staff123     (Nhân viên)
  customer / customer123  (Khách hàng)
