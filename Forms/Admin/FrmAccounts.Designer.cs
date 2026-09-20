#nullable enable
namespace QuanLyThueSanTheThao.Forms.Admin;
partial class FrmAccounts
{
    private System.ComponentModel.IContainer? components=null;private Panel top=null!;private DataGridView grid=null!;private TextBox txtSearch=null!;private Button btnRefresh=null!,btnToggle=null!,btnReset=null!;
    protected override void Dispose(bool disposing){if(disposing&&components!=null)components.Dispose();base.Dispose(disposing);}
    private void InitializeComponent(){top=new Panel();grid=new DataGridView();txtSearch=new TextBox();btnRefresh=new Button();btnToggle=new Button();btnReset=new Button();SuspendLayout();top.Dock=DockStyle.Top;top.Height=60;top.BackColor=Color.White;txtSearch.PlaceholderText="Tìm tài khoản...";txtSearch.Location=new Point(12,14);txtSearch.Width=260;btnRefresh.Text="Làm mới";btnRefresh.Location=new Point(282,11);btnRefresh.Size=new Size(90,36);btnToggle.Text="Khóa/Mở";btnToggle.Location=new Point(382,11);btnToggle.Size=new Size(100,36);btnReset.Text="Đặt lại mật khẩu";btnReset.Location=new Point(492,11);btnReset.Size=new Size(140,36);top.Controls.AddRange(new Control[]{txtSearch,btnRefresh,btnToggle,btnReset});grid.Dock=DockStyle.Fill;Controls.Add(grid);Controls.Add(top);BackColor=Color.FromArgb(243,249,248);ResumeLayout(false);}
}
