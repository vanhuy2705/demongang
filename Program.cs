using QuanLyThueSanTheThao.Forms.Auth;
using QuanLyThueSanTheThao.Helpers;

namespace QuanLyThueSanTheThao;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.SetDefaultFont(new Font("Segoe UI", 10F));
        Application.ThreadException += (_, e) =>
            UiMsg.Error(e.Exception.Message, "Lỗi ứng dụng");

        Application.Run(new FrmLogin());
    }
}
