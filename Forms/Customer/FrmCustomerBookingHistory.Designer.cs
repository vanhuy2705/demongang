#nullable enable
namespace QuanLyThueSanTheThao.Forms.Customer;
partial class FrmCustomerBookingHistory
{
    private System.ComponentModel.IContainer? components=null;private Panel top=null!;private ComboBox cboStatus=null!;private Button btnRefresh=null!,btnPay=null!,btnCancel=null!;private DataGridView grid=null!;
    protected override void Dispose(bool disposing){if(disposing&&components!=null)components.Dispose();base.Dispose(disposing);}
    private void InitializeComponent(){top=new Panel();cboStatus=new ComboBox();btnRefresh=new Button();btnPay=new Button();btnCancel=new Button();grid=new DataGridView();SuspendLayout();top.Dock=DockStyle.Top;top.Height=60;top.BackColor=Color.White;cboStatus.Location=new Point(12,14);cboStatus.Width=170;cboStatus.DropDownStyle=ComboBoxStyle.DropDownList;btnRefresh.Text="Làm mới";btnRefresh.Location=new Point(192,11);btnRefresh.Size=new Size(90,36);btnPay.Text="Thanh toán";btnPay.Location=new Point(292,11);btnPay.Size=new Size(110,36);btnCancel.Text="Hủy đặt sân";btnCancel.Location=new Point(412,11);btnCancel.Size=new Size(110,36);top.Controls.AddRange(new Control[]{cboStatus,btnRefresh,btnPay,btnCancel});grid.Dock=DockStyle.Fill;Controls.Add(grid);Controls.Add(top);BackColor=Color.FromArgb(243,249,248);ResumeLayout(false);}
}
