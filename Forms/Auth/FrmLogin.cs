using QuanLyThueSanTheThao.Forms.Common;
using QuanLyThueSanTheThao.Helpers;
using QuanLyThueSanTheThao.Services;

namespace QuanLyThueSanTheThao.Forms.Auth;

public partial class FrmLogin : Form
{
    private readonly AuthService _auth = new();

    public FrmLogin()
    {
        InitializeComponent();
        btnLogin.Click += BtnLogin_Click;
        btnEye.Click += (_,__) => txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
        lnkForgot.Click += (_,__) => MessageBox.Show("Vui lòng liên hệ quản trị viên để đặt lại mật khẩu tài khoản.","Quên mật khẩu",MessageBoxButtons.OK,MessageBoxIcon.Information);
        txtPassword.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) BtnLogin_Click(this, EventArgs.Empty); };
        var remembered = RememberMeStore.Load();
        txtUsername.Text = remembered.username;
        txtPassword.Text = remembered.password;
        chkRemember.Checked = remembered.enabled;
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        try
        {
            btnLogin.Enabled = false;
            var result = _auth.Login(txtUsername.Text, txtPassword.Text);
            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SessionContext.UserId = result.UserId;
            SessionContext.CustomerId = result.CustomerId;
            SessionContext.EmployeeId = result.EmployeeId;
            SessionContext.Username = result.Username;
            SessionContext.FullName = result.FullName;
            SessionContext.Role = result.Role;
            RememberMeStore.Save(txtUsername.Text.Trim(), txtPassword.Text, chkRemember.Checked);

            Hide();
            using var main = new FrmMainShell();
            main.ShowDialog();
            SessionContext.Clear();
            Show();
            txtPassword.Focus();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể kết nối CSDL. Hãy chạy file SQL của QuanLySanTheThaoDB và kiểm tra Data\\DatabaseConfig.cs.\n\n" + ex.Message,
                "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { btnLogin.Enabled = true; }
    }
}
