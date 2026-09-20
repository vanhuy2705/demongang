#nullable enable
namespace QuanLyThueSanTheThao.Forms.Customer;
partial class FrmCustomerInvoices
{
    private System.ComponentModel.IContainer? components=null;private DataGridView grid=null!;private Panel top=null!;private Button btnRefresh=null!,btnPay=null!;
    protected override void Dispose(bool disposing){if(disposing&&components!=null)components.Dispose();base.Dispose(disposing);}
    private void InitializeComponent(){grid=new DataGridView();top=new Panel();btnRefresh=new Button();btnPay=new Button();SuspendLayout();top.Dock=DockStyle.Top;top.Height=60;top.BackColor=Color.White;btnRefresh.Text="Làm mới";btnRefresh.Location=new Point(12,11);btnRefresh.Size=new Size(90,36);btnPay.Text="Thanh toán";btnPay.Location=new Point(112,11);btnPay.Size=new Size(110,36);top.Controls.AddRange(new Control[]{btnRefresh,btnPay});grid.Dock=DockStyle.Fill;Controls.Add(grid);Controls.Add(top);BackColor=Color.FromArgb(243,249,248);ResumeLayout(false);}
}
