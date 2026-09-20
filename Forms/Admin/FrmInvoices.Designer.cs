#nullable enable
namespace QuanLyThueSanTheThao.Forms.Admin;
partial class FrmInvoices
{
    private System.ComponentModel.IContainer? components=null;private Panel top=null!;private TextBox txtSearch=null!;private ComboBox cboStatus=null!;private Button btnRefresh=null!,btnPay=null!,btnConfirmTransfer=null!;private DataGridView grid=null!;
    protected override void Dispose(bool disposing){if(disposing&&components!=null)components.Dispose();base.Dispose(disposing);}
    private void InitializeComponent(){top=new Panel();txtSearch=new TextBox();cboStatus=new ComboBox();btnRefresh=new Button();btnPay=new Button();btnConfirmTransfer=new Button();grid=new DataGridView();SuspendLayout();top.Dock=DockStyle.Top;top.Height=60;top.BackColor=Color.White;txtSearch.PlaceholderText="Tìm hóa đơn/khách hàng...";txtSearch.Location=new Point(12,14);txtSearch.Width=260;cboStatus.Location=new Point(282,14);cboStatus.Width=150;cboStatus.DropDownStyle=ComboBoxStyle.DropDownList;btnRefresh.Text="Làm mới";btnRefresh.Location=new Point(442,11);btnRefresh.Size=new Size(90,36);btnPay.Text="Thanh toán";btnPay.Location=new Point(542,11);btnPay.Size=new Size(105,36);btnConfirmTransfer.Text="Xác nhận chuyển khoản";btnConfirmTransfer.Location=new Point(657,11);btnConfirmTransfer.Size=new Size(170,36);top.Controls.AddRange(new Control[]{txtSearch,cboStatus,btnRefresh,btnPay,btnConfirmTransfer});grid.Dock=DockStyle.Fill;Controls.Add(grid);Controls.Add(top);BackColor=Color.FromArgb(243,249,248);ResumeLayout(false);}
}
