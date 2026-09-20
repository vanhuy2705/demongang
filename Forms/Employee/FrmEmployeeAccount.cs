using Microsoft.Data.SqlClient;
using QuanLyThueSanTheThao.Data;
using QuanLyThueSanTheThao.Forms.Common;
using QuanLyThueSanTheThao.Helpers;

namespace QuanLyThueSanTheThao.Forms.Employee;

public sealed class FrmEmployeeAccount : Form
{
    private readonly TextBox txtCode = new();
    private readonly TextBox txtName = new();
    private readonly TextBox txtPhone = new();
    private readonly TextBox txtEmail = new();
    private readonly TextBox txtPosition = new();
    private readonly RoundedButton btnSave = new();
    private readonly RoundedButton btnPassword = new();

    public FrmEmployeeAccount()
    {
        BackColor=AppTheme.Background;
        var card=new RoundedPanel{Dock=DockStyle.Top,Height=430,Radius=16,BorderColor=AppTheme.Border,Padding=new Padding(26),Margin=new Padding(6)};
        var title=new Label{Text="Thông tin tài khoản",AutoSize=true,Font=new Font("Segoe UI Semibold",18F,FontStyle.Bold),ForeColor=AppTheme.Text,Location=new Point(26,22)};
        var sub=new Label{Text="Cập nhật thông tin cá nhân và bảo mật tài khoản nhân viên.",AutoSize=true,Font=new Font("Segoe UI",8.7F),ForeColor=AppTheme.Muted,Location=new Point(28,58)};
        card.Controls.AddRange(new Control[]{title,sub});
        int y=104;
        AddField(card,"Mã nhân viên",txtCode,28,ref y);txtCode.ReadOnly=true;
        AddField(card,"Họ tên",txtName,28,ref y);
        AddField(card,"Số điện thoại",txtPhone,28,ref y);
        int y2=104;
        AddField(card,"Email",txtEmail,410,ref y2);
        AddField(card,"Chức vụ",txtPosition,410,ref y2);txtPosition.ReadOnly=true;
        btnSave.Text="Lưu thay đổi";btnSave.Location=new Point(410,260);btnSave.Size=new Size(150,42);AppTheme.StylePrimary(btnSave);
        btnPassword.Text="Đổi mật khẩu";btnPassword.Location=new Point(570,260);btnPassword.Size=new Size(150,42);AppTheme.StyleSecondary(btnPassword);
        card.Controls.AddRange(new Control[]{btnSave,btnPassword});
        Controls.Add(card);
        AppTheme.Upgrade(this);
        btnSave.Click+=(_,__)=>Save();btnPassword.Click+=(_,__)=>ChangePassword();Shown+=(_,__)=>LoadData();
    }
    private static void AddField(Control parent,string label,Control control,int x,ref int y){var l=new Label{Text=label,AutoSize=true,Font=new Font("Segoe UI Semibold",8.7F),ForeColor=AppTheme.Text,Location=new Point(x,y)};control.Location=new Point(x,y+24);control.Size=new Size(330,30);AppTheme.StyleInput(control);parent.Controls.AddRange(new Control[]{l,control});y+=72;}
    private void LoadData(){var dt=Db.Query("SELECT MaNhanVien,HoTen,SoDienThoai,Email,ChucVu FROM NhanVien WHERE NhanVienID=@id",new SqlParameter("@id",SessionContext.EmployeeId??0));if(dt.Rows.Count==0)return;var r=dt.Rows[0];txtCode.Text=Convert.ToString(r["MaNhanVien"]);txtName.Text=Convert.ToString(r["HoTen"]);txtPhone.Text=Convert.ToString(r["SoDienThoai"]);txtEmail.Text=Convert.ToString(r["Email"]);txtPosition.Text=Convert.ToString(r["ChucVu"]);}
    private void Save(){if(string.IsNullOrWhiteSpace(txtName.Text)){UiMsg.Warn("Họ tên không được để trống.");return;}Db.Execute("UPDATE NhanVien SET HoTen=@n,SoDienThoai=@p,Email=@e WHERE NhanVienID=@id",new SqlParameter("@n",txtName.Text.Trim()),new SqlParameter("@p",txtPhone.Text.Trim()),new SqlParameter("@e",txtEmail.Text.Trim()),new SqlParameter("@id",SessionContext.EmployeeId??0));SessionContext.FullName=txtName.Text.Trim();Toast.Success("Đã cập nhật thông tin.");}
    private void ChangePassword(){using var f=new Form{Text="Đổi mật khẩu",StartPosition=FormStartPosition.CenterParent,Size=new Size(450,290),FormBorderStyle=FormBorderStyle.FixedDialog,MaximizeBox=false,MinimizeBox=false,BackColor=AppTheme.Background};var card=new RoundedPanel{Dock=DockStyle.Fill,Padding=new Padding(24),Radius=14,BorderColor=AppTheme.Border};var oldP=new TextBox{Left=26,Top=40,Width=360,PlaceholderText="Mật khẩu hiện tại",UseSystemPasswordChar=true};var newP=new TextBox{Left=26,Top=88,Width=360,PlaceholderText="Mật khẩu mới",UseSystemPasswordChar=true};var confirm=new TextBox{Left=26,Top=136,Width=360,PlaceholderText="Nhập lại mật khẩu mới",UseSystemPasswordChar=true};var b=new RoundedButton{Left=236,Top=188,Width=150,Height=40,Text="Đổi mật khẩu"};AppTheme.StylePrimary(b);foreach(var c in new Control[]{oldP,newP,confirm})AppTheme.StyleInput(c);b.Click+=(_,__)=>{var dt=Db.Query("SELECT MatKhauBam,MuoiMatKhau FROM TaiKhoan WHERE TaiKhoanID=@id",new SqlParameter("@id",SessionContext.UserId));if(dt.Rows.Count==0)return;var salt=Convert.ToString(dt.Rows[0]["MuoiMatKhau"])??"";if(!string.Equals(Convert.ToString(dt.Rows[0]["MatKhauBam"]),PasswordHelper.Hash(salt,oldP.Text),StringComparison.OrdinalIgnoreCase)){UiMsg.Warn("Mật khẩu hiện tại không đúng.");return;}if(newP.Text.Length<6||newP.Text!=confirm.Text){UiMsg.Warn("Mật khẩu mới tối thiểu 6 ký tự và phải nhập lại chính xác.");return;}var ns=Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();Db.Execute("UPDATE TaiKhoan SET MuoiMatKhau=@s,MatKhauBam=@h WHERE TaiKhoanID=@id",new SqlParameter("@s",ns),new SqlParameter("@h",PasswordHelper.Hash(ns,newP.Text)),new SqlParameter("@id",SessionContext.UserId));Toast.Success("Đã đổi mật khẩu.");f.Close();};card.Controls.AddRange(new Control[]{oldP,newP,confirm,b});f.Controls.Add(card);AppTheme.Upgrade(f);f.ShowDialog(this);}
}
