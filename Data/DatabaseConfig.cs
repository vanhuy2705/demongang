namespace QuanLyThueSanTheThao.Data;

public static class DatabaseConfig
{
    // SQL Server 2022 Express theo cấu hình đã chốt.
    public const string SqlServerInstance = @"localhost\SQLEXPRESS";
    public const string DatabaseName = "QuanLySanTheThaoDB";

    // Đây là nơi SQL Server lưu file vật lý MDF/LDF.
    // WinForms KHÔNG mở trực tiếp file MDF khi database đã được attach vào SQLEXPRESS;
    // ứng dụng kết nối qua instance + tên database để tránh lỗi attach trùng database.
    public const string DatabaseDataDirectory = @"C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA";
    public const string DatabaseFilePath = DatabaseDataDirectory + @"\QuanLySanTheThaoDB.mdf";
    public const string DatabaseLogFilePath = DatabaseDataDirectory + @"\QuanLySanTheThaoDB_log.ldf";

    public static string ConnectionString { get; set; } =
        $@"Server={SqlServerInstance};Database={DatabaseName};Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Connect Timeout=15;";
}
