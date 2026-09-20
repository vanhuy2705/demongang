using QuanLyThueSanTheThao.Helpers;using QuanLyThueSanTheThao.Services;
namespace QuanLyThueSanTheThao.Forms.Admin;
public partial class FrmSettings:Form
{
    private readonly SettingsService s=new();public FrmSettings(){InitializeComponent();AppTheme.StylePrimary(btnSave);btnSave.Click+=(_,__)=>Save();Shown+=(_,__)=>LoadData();}
    private void LoadData(){txtCompany.Text=s.Get("CompanyName");txtBankName.Text=s.Get("BankName");txtBankBin.Text=s.Get("BankBin");txtAccount.Text=s.Get("BankAccountNumber");txtAccountName.Text=s.Get("BankAccountName");txtPrefix.Text=s.Get("QrTransferPrefix","THANHTOAN");txtOpen.Text=s.Get("OpenTime","05:00");txtClose.Text=s.Get("CloseTime","23:00");}
    private void Save(){if(string.IsNullOrWhiteSpace(txtBankBin.Text)||string.IsNullOrWhiteSpace(txtAccount.Text)){MessageBox.Show("Cần nhập Bank BIN và số tài khoản để tạo QR thanh toán.");return;}s.Set("CompanyName",txtCompany.Text.Trim());s.Set("BankName",txtBankName.Text.Trim());s.Set("BankBin",txtBankBin.Text.Trim());s.Set("BankAccountNumber",txtAccount.Text.Trim());s.Set("BankAccountName",txtAccountName.Text.Trim().ToUpperInvariant());s.Set("QrTransferPrefix",txtPrefix.Text.Trim().ToUpperInvariant());s.Set("OpenTime",txtOpen.Text.Trim());s.Set("CloseTime",txtClose.Text.Trim());MessageBox.Show("Đã lưu cấu hình.");}
}
