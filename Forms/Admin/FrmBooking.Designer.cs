#nullable enable
namespace QuanLyThueSanTheThao.Forms.Admin;
partial class FrmBooking
{
    private System.ComponentModel.IContainer? components=null;
    private QuanLyThueSanTheThao.Forms.Common.RoundedPanel card=null!;
    private ComboBox cboCustomer=null!,cboType=null!,cboField=null!,cboVoucher=null!;
    private DateTimePicker dtDate=null!,dtStart=null!,dtEnd=null!;private TextBox txtNote=null!;
    private Label lblPrice=null!,lblSubtotal=null!,lblDiscount=null!,lblTotal=null!,lblAvailability=null!;
    private QuanLyThueSanTheThao.Forms.Common.RoundedButton btnCheck=null!,btnBook=null!;
    protected override void Dispose(bool disposing){if(disposing&&components!=null)components.Dispose();base.Dispose(disposing);}
    private void InitializeComponent()
    {
        card=new QuanLyThueSanTheThao.Forms.Common.RoundedPanel();cboCustomer=new ComboBox();cboType=new ComboBox();cboField=new ComboBox();cboVoucher=new ComboBox();dtDate=new DateTimePicker();dtStart=new DateTimePicker();dtEnd=new DateTimePicker();txtNote=new TextBox();lblPrice=new Label();lblSubtotal=new Label();lblDiscount=new Label();lblTotal=new Label();lblAvailability=new Label();btnCheck=new QuanLyThueSanTheThao.Forms.Common.RoundedButton();btnBook=new QuanLyThueSanTheThao.Forms.Common.RoundedButton();SuspendLayout();
        BackColor=Color.FromArgb(239,248,252);Padding=new Padding(10);AutoScaleMode=AutoScaleMode.Dpi;
        card.BackColor=Color.White;card.Dock=DockStyle.Top;card.Height=590;card.Padding=new Padding(26);card.Radius=16;card.BorderColor=Color.FromArgb(218,232,240);Controls.Add(card);
        var title=new Label{Text="Đặt sân thể thao",Font=new Font("Segoe UI Semibold",19F,FontStyle.Bold),ForeColor=Color.FromArgb(18,53,78),AutoSize=true,Location=new Point(28,20)};
        var sub=new Label{Text="Chọn sân và khung giờ phù hợp. Hệ thống sẽ tự kiểm tra trùng lịch và tính tiền.",Font=new Font("Segoe UI",8.5F),ForeColor=Color.FromArgb(99,122,141),AutoSize=true,Location=new Point(30,56)};
        var sep=new Panel{BackColor=Color.FromArgb(232,239,243),Location=new Point(28,83),Height=1,Width=870,Anchor=AnchorStyles.Left|AnchorStyles.Top|AnchorStyles.Right};
        card.Controls.AddRange(new Control[]{title,sub,sep});
        var left=new QuanLyThueSanTheThao.Forms.Common.RoundedPanel{BackColor=Color.FromArgb(250,253,254),BorderColor=Color.FromArgb(229,237,242),Radius=13,Location=new Point(28,104),Size=new Size(410,430),Padding=new Padding(18)};
        var right=new QuanLyThueSanTheThao.Forms.Common.RoundedPanel{BackColor=Color.FromArgb(250,253,254),BorderColor=Color.FromArgb(229,237,242),Radius=13,Location=new Point(456,104),Size=new Size(442,430),Padding=new Padding(18)};
        card.Controls.AddRange(new Control[]{left,right});
        var ltitle=new Label{Text="Thông tin đặt sân",AutoSize=true,Font=new Font("Segoe UI Semibold",11F,FontStyle.Bold),ForeColor=Color.FromArgb(22,75,132),Location=new Point(18,14)};left.Controls.Add(ltitle);
        var rtitle=new Label{Text="Thời gian & thanh toán",AutoSize=true,Font=new Font("Segoe UI Semibold",11F,FontStyle.Bold),ForeColor=Color.FromArgb(22,75,132),Location=new Point(18,14)};right.Controls.Add(rtitle);
        var y=54;void AddL(string label,Control c){var lb=new Label{Text=label,AutoSize=true,Font=new Font("Segoe UI Semibold",8.3F),ForeColor=Color.FromArgb(39,67,88),Location=new Point(18,y)};c.Location=new Point(18,y+22);c.Width=360;c.Height=30;left.Controls.AddRange(new Control[]{lb,c});y+=68;}
        AddL("Khách hàng",cboCustomer);AddL("Loại sân",cboType);AddL("Sân",cboField);AddL("Voucher",cboVoucher);AddL("Ghi chú",txtNote);txtNote.Multiline=true;txtNote.Height=54;
        var y2=54;void AddR(string label,Control c){var lb=new Label{Text=label,AutoSize=true,Font=new Font("Segoe UI Semibold",8.3F),ForeColor=Color.FromArgb(39,67,88),Location=new Point(18,y2)};c.Location=new Point(18,y2+22);c.Width=190;c.Height=30;right.Controls.AddRange(new Control[]{lb,c});y2+=66;}
        AddR("Ngày đặt",dtDate);AddR("Giờ bắt đầu",dtStart);AddR("Giờ kết thúc",dtEnd);
        btnCheck.Text="Kiểm tra sân trống";btnCheck.Location=new Point(224,76);btnCheck.Size=new Size(185,38);btnCheck.BackColor=Color.FromArgb(236,248,246);btnCheck.ForeColor=Color.FromArgb(0,135,100);btnCheck.Radius=10;btnCheck.HoverColor=Color.FromArgb(220,242,236);btnCheck.Font=new Font("Segoe UI Semibold",8.5F,FontStyle.Bold);right.Controls.Add(btnCheck);
        lblAvailability.AutoSize=false;lblAvailability.Size=new Size(390,40);lblAvailability.Location=new Point(18,253);lblAvailability.ForeColor=Color.FromArgb(0,145,95);lblAvailability.Font=new Font("Segoe UI Semibold",8.3F);right.Controls.Add(lblAvailability);
        lblPrice.Location=new Point(18,296);lblPrice.AutoSize=true;lblPrice.ForeColor=Color.FromArgb(91,113,130);lblSubtotal.Location=new Point(18,321);lblSubtotal.AutoSize=true;lblSubtotal.ForeColor=Color.FromArgb(91,113,130);lblDiscount.Location=new Point(18,346);lblDiscount.AutoSize=true;lblDiscount.ForeColor=Color.FromArgb(241,120,31);lblTotal.Location=new Point(18,374);lblTotal.AutoSize=true;lblTotal.Font=new Font("Segoe UI Semibold",15F,FontStyle.Bold);lblTotal.ForeColor=Color.FromArgb(0,150,100);right.Controls.AddRange(new Control[]{lblPrice,lblSubtotal,lblDiscount,lblTotal});
        btnBook.Text="✓  Xác nhận đặt sân";btnBook.Location=new Point(224,366);btnBook.Size=new Size(185,44);btnBook.BackColor=Color.FromArgb(19,198,119);btnBook.ForeColor=Color.White;btnBook.Radius=10;btnBook.HoverColor=Color.FromArgb(8,158,89);btnBook.Font=new Font("Segoe UI Semibold",9F,FontStyle.Bold);right.Controls.Add(btnBook);
        card.Resize+=(_,__)=>{sep.Width=card.Width-56;right.Width=Math.Max(360,card.Width-right.Left-28);};
        ResumeLayout(false);
    }
}
