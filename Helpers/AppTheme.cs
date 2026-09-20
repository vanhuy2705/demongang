using QuanLyThueSanTheThao.Forms.Common;

namespace QuanLyThueSanTheThao.Helpers;

public static class AppTheme
{
    public static readonly Color Sidebar = Color.FromArgb(5, 29, 39);
    public static readonly Color Sidebar2 = Color.FromArgb(9, 46, 56);
    public static readonly Color SidebarSoft = Color.FromArgb(17, 57, 61);
    public static readonly Color Accent = Color.FromArgb(19, 198, 119);
    public static readonly Color Accent2 = Color.FromArgb(19, 170, 157);
    public static readonly Color AccentDark = Color.FromArgb(7, 145, 96);
    public static readonly Color Background = Color.FromArgb(244, 250, 252);
    public static readonly Color BackgroundBlue = Color.FromArgb(239, 248, 253);
    public static readonly Color Surface = Color.White;
    public static readonly Color Text = Color.FromArgb(18, 53, 78);
    public static readonly Color Muted = Color.FromArgb(103, 126, 145);
    public static readonly Color Border = Color.FromArgb(219, 232, 240);
    public static readonly Color Success = Color.FromArgb(27, 184, 105);
    public static readonly Color Warning = Color.FromArgb(255, 159, 24);
    public static readonly Color Danger = Color.FromArgb(241, 82, 82);
    public static readonly Color Info = Color.FromArgb(39, 126, 244);
    public static readonly Color Purple = Color.FromArgb(132, 82, 236);
    public static readonly Font Title = new("Segoe UI Semibold", 20F, FontStyle.Bold);
    public static readonly Font SectionTitle = new("Segoe UI Semibold", 11.5F, FontStyle.Bold);

    public static void StylePrimary(Button b)
    {
        b.BackColor = Accent;
        b.ForeColor = Color.White;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.Cursor = Cursors.Hand;
        b.Font = new Font("Segoe UI Semibold", 9.3F, FontStyle.Bold);
        b.Height = Math.Max(b.Height, 38);
        if (b is RoundedButton rb)
        {
            rb.Radius = 10;
            rb.HoverColor = AccentDark;
        }
    }

    public static void StyleSecondary(Button b)
    {
        b.BackColor = Color.FromArgb(239, 248, 247);
        b.ForeColor = AccentDark;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderColor = Color.FromArgb(187, 224, 216);
        b.FlatAppearance.BorderSize = 1;
        b.Cursor = Cursors.Hand;
        b.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        b.Height = Math.Max(b.Height, 36);
        if (b is RoundedButton rb)
        {
            rb.Radius = 10;
            rb.HoverColor = Color.FromArgb(222, 243, 237);
        }
    }

    public static void StyleDanger(Button b)
    {
        b.BackColor = Danger;
        b.ForeColor = Color.White;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.Cursor = Cursors.Hand;
        b.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
    }

    public static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.RowHeadersVisible = false;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(233, 244, 250);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Text;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(233, 244, 250);
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        grid.ColumnHeadersHeight = 36;
        grid.DefaultCellStyle.BackColor = Color.White;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(251, 253, 254);
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 247, 237);
        grid.DefaultCellStyle.SelectionForeColor = Text;
        grid.DefaultCellStyle.ForeColor = Text;
        grid.DefaultCellStyle.Font = new Font("Segoe UI", 8.8F);
        grid.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
        grid.RowTemplate.Height = 34;
        grid.GridColor = Color.FromArgb(232, 239, 242);
    }

    public static void StyleInput(Control c)
    {
        c.Font = new Font("Segoe UI", 9.2F);
        c.ForeColor = Text;
        if (c is TextBox tb)
        {
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.BackColor = Color.White;
        }
        else if (c is ComboBox cb)
        {
            cb.FlatStyle = FlatStyle.Flat;
            cb.BackColor = Color.White;
        }
        else if (c is DateTimePicker dt)
        {
            dt.CalendarForeColor = Text;
            dt.CalendarMonthBackground = Color.White;
        }
    }

    public static void ApplyToForm(Form form, string role = "")
    {
        form.BackColor = role == "Customer" ? BackgroundBlue : Background;
        foreach (Control c in form.Controls) ApplyRecursive(c);
    }

    private static void ApplyRecursive(Control c)
    {
        switch (c)
        {
            case DataGridView grid:
                StyleGrid(grid);
                break;
            case RoundedButton:
                break;
            case Button b:
                if (IsDangerButton(b.Text)) StyleDanger(b);
                else if (IsPrimaryButton(b.Text)) StylePrimary(b);
                else StyleSecondary(b);
                break;
            case TextBox or ComboBox or DateTimePicker:
                StyleInput(c);
                break;
            case Label l when l.Font.Size <= 11.5f:
                if (l.ForeColor == SystemColors.ControlText || l.ForeColor == Color.Black)
                    l.ForeColor = Text;
                break;
            case SplitContainer s:
                s.BackColor = Background;
                break;
            case Panel p when p is not RoundedPanel && p.BackColor.ToArgb() == Color.White.ToArgb():
                p.BackColor = Color.White;
                ApplyRoundedRegion(p, 12);
                p.Resize += (_,__) => ApplyRoundedRegion(p, 12);
                break;
        }
        foreach (Control child in c.Controls) ApplyRecursive(child);
    }

    private static void ApplyRoundedRegion(Control c, int radius)
    {
        if (c.Width < 4 || c.Height < 4) return;
        using var path = RoundedPanel.CreateRoundPath(new Rectangle(0,0,c.Width,c.Height), radius);
        c.Region?.Dispose();
        c.Region = new Region(path);
    }

    private static bool IsPrimaryButton(string text)
    {
        text = text.ToLowerInvariant();
        return text.Contains("lưu") || text.Contains("thêm") || text.Contains("xác nhận") ||
               text.Contains("đặt sân") || text.Contains("thanh toán") || text.Contains("đăng nhập") ||
               text.Contains("tạo") || text.Contains("áp dụng");
    }

    private static bool IsDangerButton(string text)
    {
        text = text.ToLowerInvariant();
        return text.Contains("xóa") || text.Contains("hủy đơn");
    }

    public static string BookingStatusVi(object? status)
    {
        var s = Convert.ToString(status) ?? "";
        return s switch
        {
            "Pending" => "Chờ xác nhận",
            "Confirmed" => "Đã xác nhận",
            "InUse" => "Đang sử dụng",
            "Completed" => "Hoàn thành",
            "Cancelled" => "Đã hủy",
            _ => s
        };
    }

    public static string PaymentStatusVi(object? status)
    {
        var s = Convert.ToString(status) ?? "";
        return s switch
        {
            "Unpaid" => "Chưa thanh toán",
            "PartiallyPaid" => "Thanh toán một phần",
            "Paid" => "Đã thanh toán",
            "Pending" => "Chờ xác nhận",
            "Confirmed" => "Đã xác nhận",
            _ => s
        };
    }
}
