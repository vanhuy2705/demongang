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
            MessageBox.Show(e.Exception.Message, "Lỗi ứng dụng", MessageBoxButtons.OK, MessageBoxIcon.Error);

        Application.Run(new FrmLogin());
    }
}
