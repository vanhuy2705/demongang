#nullable enable
namespace QuanLyThueSanTheThao.Forms.Customer;
partial class FrmCustomerVouchers
{
    private System.ComponentModel.IContainer? components=null;private FlowLayoutPanel flow=null!;private Label title=null!;
    protected override void Dispose(bool disposing){if(disposing&&components!=null)components.Dispose();base.Dispose(disposing);}
    private void InitializeComponent(){flow=new FlowLayoutPanel();title=new Label();SuspendLayout();title.Text="Voucher của bạn";title.Dock=DockStyle.Top;title.Height=55;title.Font=new Font("Segoe UI Semibold",18F,FontStyle.Bold);title.ForeColor=Color.FromArgb(25,54,72);title.TextAlign=ContentAlignment.MiddleLeft;flow.Dock=DockStyle.Fill;flow.AutoScroll=true;flow.Padding=new Padding(0,8,0,0);Controls.Add(flow);Controls.Add(title);BackColor=Color.FromArgb(243,249,248);ResumeLayout(false);}
}
