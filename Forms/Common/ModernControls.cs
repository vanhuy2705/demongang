using System.Drawing.Drawing2D;
using QuanLyThueSanTheThao.Helpers;

namespace QuanLyThueSanTheThao.Forms.Common;

public class RoundedPanel : Panel
{
    public int Radius { get; set; } = 16;
    public Color BorderColor { get; set; } = Color.FromArgb(224, 235, 239);
    public int BorderThickness { get; set; } = 1;
    public bool DrawShadow { get; set; } = false;

    public RoundedPanel()
    {
        DoubleBuffered = true;
        BackColor = Color.White;
        ResizeRedraw = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var r = ClientRectangle;
        r.Width -= 1; r.Height -= 1;
        if (r.Width <= 1 || r.Height <= 1) return;
        using var path = CreateRoundPath(r, Radius);
        Region = new Region(path);
        if (DrawShadow)
        {
            using var shadow = new SolidBrush(Color.FromArgb(18, 15, 23, 42));
            var sr = new Rectangle(r.X + 2, r.Y + 3, Math.Max(1, r.Width - 3), Math.Max(1, r.Height - 4));
            using var sp = CreateRoundPath(sr, Radius);
            e.Graphics.FillPath(shadow, sp);
        }
        using var b = new SolidBrush(BackColor);
        e.Graphics.FillPath(b, path);
        if (BorderThickness > 0)
        {
            using var p = new Pen(BorderColor, BorderThickness);
            e.Graphics.DrawPath(p, path);
        }
    }

    internal static GraphicsPath CreateRoundPath(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        int d = Math.Max(2, Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height)));
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}

public class RoundedButton : Button
{
    public int Radius { get; set; } = 12;
    public Color HoverColor { get; set; } = Color.Empty;
    private Color _baseColor;

    public RoundedButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Cursor = Cursors.Hand;
        UseVisualStyleBackColor = false;
        Resize += (_, _) => UpdateRegion();
        MouseEnter += (_, _) => { _baseColor = BackColor; if (HoverColor != Color.Empty) BackColor = HoverColor; };
        MouseLeave += (_, _) => { if (_baseColor != Color.Empty) BackColor = _baseColor; };
    }

    private void UpdateRegion()
    {
        if (Width <= 2 || Height <= 2) return;
        using var path = RoundedPanel.CreateRoundPath(new Rectangle(0, 0, Width, Height), Radius);
        Region = new Region(path);
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        UpdateRegion();
    }
}

public sealed class IconBadge : Control
{
    public string Glyph { get; set; } = "●";
    public Color AccentColor { get; set; } = AppTheme.Accent;
    public bool Filled { get; set; } = true;

    public IconBadge()
    {
        Size = new Size(42, 42);
        DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var rect = new Rectangle(1, 1, Width - 3, Height - 3);
        using var b = new SolidBrush(Filled ? AccentColor : Color.FromArgb(30, AccentColor));
        e.Graphics.FillEllipse(b, rect);
        using var font = new Font("Segoe UI Symbol", Math.Max(10, Height * .35f), FontStyle.Bold, GraphicsUnit.Pixel);
        using var tb = new SolidBrush(Filled ? Color.White : AccentColor);
        var sz = e.Graphics.MeasureString(Glyph, font);
        e.Graphics.DrawString(Glyph, font, tb, (Width - sz.Width) / 2f, (Height - sz.Height) / 2f - 1);
    }
}

public sealed class SportsHeroPanel : Panel
{
    public string Heading { get; set; } = "SportField";
    public string Subheading { get; set; } = "Đặt sân dễ dàng - Chơi hết mình";
    public bool Compact { get; set; }

    public SportsHeroPanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var bg = new LinearGradientBrush(ClientRectangle, Color.FromArgb(4, 28, 38), Color.FromArgb(6, 77, 74), 15f);
        g.FillRectangle(bg, ClientRectangle);

        // stadium light / sky glow
        using var glow = new LinearGradientBrush(new Rectangle(0, 0, Width, Math.Max(1, Height / 2)), Color.FromArgb(55, 76, 169, 213), Color.Transparent, 90f);
        g.FillRectangle(glow, 0, 0, Width, Math.Max(1, Height / 2));

        // football field perspective
        int horizon = (int)(Height * .53);
        using var turf = new LinearGradientBrush(new Rectangle(0, horizon, Width, Math.Max(1, Height - horizon)), Color.FromArgb(25, 83, 62), Color.FromArgb(5, 43, 35), 90f);
        g.FillRectangle(turf, 0, horizon, Width, Height - horizon);
        using var line = new Pen(Color.FromArgb(85, 220, 255, 235), 2f);
        for (int i = 0; i <= 5; i++)
        {
            float x = Width * (i / 5f);
            g.DrawLine(line, Width / 2f, horizon, x, Height);
        }
        for (int i = 1; i <= 4; i++)
        {
            float y = horizon + (Height - horizon) * (i / 5f);
            float inset = (Height - y) * .38f;
            g.DrawLine(line, inset, y, Width - inset, y);
        }
        g.DrawEllipse(line, Width * .39f, horizon + (Height - horizon) * .15f, Width * .22f, (Height - horizon) * .38f);

        // decorative green slashes
        using var slash = new SolidBrush(Color.FromArgb(150, 24, 210, 118));
        g.ResetTransform();
        var state = g.Save();
        g.TranslateTransform(Width - 110, 20);
        g.RotateTransform(-20);
        g.FillRectangle(slash, 0, 0, 18, 150);
        g.FillRectangle(slash, 30, -20, 9, 130);
        g.Restore(state);

        // ball
        float ballSize = Compact ? 64f : 92f;
        float bx = Width - ballSize - (Compact ? 32f : 56f);
        float by = Height - ballSize - 24f;
        DrawBall(g, bx, by, ballSize);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var brandFont = new Font("Segoe UI Semibold", Compact ? 18F : 25F, FontStyle.Bold);
        using var titleFont = new Font("Segoe UI Semibold", Compact ? 23F : 30F, FontStyle.Bold);
        using var subFont = new Font("Segoe UI", Compact ? 9F : 11F);
        using var white = new SolidBrush(Color.White);
        using var green = new SolidBrush(Color.FromArgb(44, 224, 125));
        using var soft = new SolidBrush(Color.FromArgb(220, 236, 247, 245));

        float x = Compact ? 24 : 42;
        g.DrawString("⚽  Sport", brandFont, white, x, Compact ? 18 : 34);
        var sportW = g.MeasureString("⚽  Sport", brandFont).Width;
        g.DrawString("Field", brandFont, green, x + sportW - 2, Compact ? 18 : 34);
        g.DrawString(Subheading, subFont, soft, x + (Compact ? 35 : 48), Compact ? 48 : 72);

        if (!Compact)
        {
            g.DrawString(Heading, titleFont, white, x, 145);
            g.DrawString("Đăng nhập để tiếp tục đặt sân và trải nghiệm\nnhững tiện ích tốt nhất.", subFont, soft, x, 194);
        }
    }

    private static void DrawBall(Graphics g, float x, float y, float size)
    {
        using var shadow = new SolidBrush(Color.FromArgb(75, 0, 0, 0));
        g.FillEllipse(shadow, x + 5, y + 7, size, size * .94f);
        using var white = new SolidBrush(Color.FromArgb(245, 248, 246));
        g.FillEllipse(white, x, y, size, size);
        using var p = new Pen(Color.FromArgb(25, 35, 40), Math.Max(1.5f, size / 45f));
        g.DrawEllipse(p, x, y, size, size);
        var pts = new[]
        {
            new PointF(x+size*.50f,y+size*.28f), new PointF(x+size*.64f,y+size*.39f),
            new PointF(x+size*.59f,y+size*.56f), new PointF(x+size*.41f,y+size*.56f),
            new PointF(x+size*.36f,y+size*.39f)
        };
        using var dark = new SolidBrush(Color.FromArgb(24, 34, 37));
        g.FillPolygon(dark, pts);
        g.DrawLine(p, pts[0], new PointF(x+size*.50f,y+size*.08f));
        g.DrawLine(p, pts[1], new PointF(x+size*.85f,y+size*.31f));
        g.DrawLine(p, pts[2], new PointF(x+size*.75f,y+size*.78f));
        g.DrawLine(p, pts[3], new PointF(x+size*.25f,y+size*.78f));
        g.DrawLine(p, pts[4], new PointF(x+size*.15f,y+size*.31f));
    }
}

public sealed class DonutStatusChart : Control
{
    public int Available { get; set; }
    public int Busy { get; set; }
    public int Maintenance { get; set; }

    public DonutStatusChart()
    {
        DoubleBuffered = true;
        BackColor = Color.White;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        int total = Math.Max(1, Available + Busy + Maintenance);
        int size = Math.Min(Width * 55 / 100, Height - 32);
        size = Math.Max(80, size);
        var rect = new Rectangle(18, (Height - size) / 2, size, size);
        using var penAvail = new Pen(AppTheme.Success, Math.Max(13, size / 7f));
        using var penBusy = new Pen(AppTheme.Info, Math.Max(13, size / 7f));
        using var penMaint = new Pen(AppTheme.Danger, Math.Max(13, size / 7f));
        penAvail.StartCap = penAvail.EndCap = LineCap.Flat;
        float start = -90;
        float a1 = 360f * Available / total;
        float a2 = 360f * Busy / total;
        float a3 = 360f * Maintenance / total;
        e.Graphics.DrawArc(penAvail, rect, start, a1); start += a1;
        e.Graphics.DrawArc(penBusy, rect, start, a2); start += a2;
        e.Graphics.DrawArc(penMaint, rect, start, a3);

        using var valueFont = new Font("Segoe UI Semibold", 17F, FontStyle.Bold);
        using var subFont = new Font("Segoe UI", 8.5F);
        using var tb = new SolidBrush(AppTheme.Text);
        using var mb = new SolidBrush(AppTheme.Muted);
        string t = total.ToString();
        var ts = e.Graphics.MeasureString(t, valueFont);
        e.Graphics.DrawString(t, valueFont, tb, rect.X + (rect.Width - ts.Width)/2, rect.Y + rect.Height*.35f);
        var ss = e.Graphics.MeasureString("Tổng số sân", subFont);
        e.Graphics.DrawString("Tổng số sân", subFont, mb, rect.X + (rect.Width - ss.Width)/2, rect.Y + rect.Height*.58f);

        int lx = rect.Right + 26;
        int ly = Math.Max(22, Height/2 - 48);
        DrawLegend(e.Graphics, lx, ly, AppTheme.Success, "Trống", Available, total);
        DrawLegend(e.Graphics, lx, ly + 34, AppTheme.Info, "Đang thuê", Busy, total);
        DrawLegend(e.Graphics, lx, ly + 68, AppTheme.Danger, "Bảo trì", Maintenance, total);
    }

    private static void DrawLegend(Graphics g, int x, int y, Color color, string label, int value, int total)
    {
        using var b = new SolidBrush(color);
        g.FillEllipse(b, x, y + 4, 11, 11);
        using var f = new Font("Segoe UI", 8.5F);
        using var tb = new SolidBrush(AppTheme.Text);
        g.DrawString(label, f, tb, x + 18, y);
        string right = $"{value} ({Math.Round(value * 100m / Math.Max(1,total))}%)";
        g.DrawString(right, f, tb, x + 88, y);
    }
}

public sealed class BookingTimelineControl : Control
{
    public sealed record Item(string Field, DateTime Start, DateTime End, string Customer, string Status);
    public List<Item> Items { get; set; } = new();
    public int StartHour { get; set; } = 6;
    public int EndHour { get; set; } = 22;

    public BookingTimelineControl()
    {
        DoubleBuffered = true;
        BackColor = Color.White;
        MinimumSize = new Size(560, 230);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var fields = Items.Select(x => x.Field).Distinct().Take(7).ToList();
        if (fields.Count == 0) fields.AddRange(new[] { "Sân A1", "Sân A2", "Sân B1", "Sân C1" });
        int left = 82, top = 34, right = 12, bottom = 12;
        int rowH = Math.Max(30, (Height - top - bottom) / Math.Max(1, fields.Count));
        float hourW = (Width - left - right) / (float)Math.Max(1, EndHour - StartHour);
        using var gridPen = new Pen(Color.FromArgb(230, 238, 242), 1);
        using var headerBrush = new SolidBrush(Color.FromArgb(249, 252, 253));
        g.FillRectangle(headerBrush, 0, 0, Width, top);
        using var small = new Font("Segoe UI", 8F);
        using var bold = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
        using var text = new SolidBrush(AppTheme.Text);
        using var muted = new SolidBrush(AppTheme.Muted);
        for (int h = StartHour; h <= EndHour; h += 2)
        {
            float x = left + (h - StartHour) * hourW;
            g.DrawLine(gridPen, x, top, x, Height - bottom);
            g.DrawString($"{h:00}:00", small, text, x + 3, 9);
        }
        for (int i = 0; i < fields.Count; i++)
        {
            int y = top + i * rowH;
            g.DrawLine(gridPen, 0, y, Width, y);
            g.DrawString(fields[i], bold, text, 12, y + 9);
            var rowItems = Items.Where(x => x.Field == fields[i]).ToList();
            foreach (var item in rowItems)
            {
                float st = Math.Max(StartHour, (float)item.Start.TimeOfDay.TotalHours);
                float en = Math.Min(EndHour, (float)item.End.TimeOfDay.TotalHours);
                if (en <= st) continue;
                float x = left + (st - StartHour) * hourW + 3;
                float w = Math.Max(34, (en - st) * hourW - 6);
                var rr = new RectangleF(x, y + 5, w, rowH - 10);
                var color = StatusColor(item.Status);
                using var fill = new SolidBrush(Color.FromArgb(50, color));
                using var border = new Pen(color, 1.2f);
                using var path = RoundedPanel.CreateRoundPath(Rectangle.Round(rr), 6);
                g.FillPath(fill, path); g.DrawPath(border, path);
                string label = item.Customer;
                if (w > 85) label += $"\n{item.Start:HH:mm}-{item.End:HH:mm}";
                using var lf = new Font("Segoe UI Semibold", w > 85 ? 7.3F : 7F, FontStyle.Bold);
                using var lb = new SolidBrush(Color.FromArgb(25, 64, 77));
                var clip = g.Clip;
                g.SetClip(rr);
                g.DrawString(label, lf, lb, rr.X + 6, rr.Y + 3);
                g.Clip = clip;
            }
        }
        g.DrawLine(gridPen, 0, top + fields.Count * rowH, Width, top + fields.Count * rowH);
    }

    private static Color StatusColor(string status) => status switch
    {
        "Cancelled" or "Đã hủy" => AppTheme.Danger,
        "Pending" or "Chờ xác nhận" => AppTheme.Warning,
        "InUse" or "Đang sử dụng" => AppTheme.Info,
        "Maintenance" or "Bảo trì" => AppTheme.Danger,
        _ => AppTheme.Success
    };
}

public sealed class MiniCalendarPanel : RoundedPanel
{
    public DateTime SelectedDate { get; set; } = DateTime.Today;
    public MiniCalendarPanel()
    {
        Radius = 14;
        BorderColor = Color.FromArgb(58, 92, 105);
        BackColor = Color.FromArgb(6, 42, 55);
        DoubleBuffered = true;
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g=e.Graphics; g.SmoothingMode=SmoothingMode.AntiAlias;
        using var title = new Font("Segoe UI Semibold",10F,FontStyle.Bold);
        using var normal = new Font("Segoe UI",8F);
        using var white=new SolidBrush(Color.White); using var soft=new SolidBrush(Color.FromArgb(205,225,230));
        g.DrawString("▣  Lịch hôm nay",title,white,16,14);
        g.DrawString(SelectedDate.ToString("dd/MM/yyyy"),normal,soft,34,38);
        string[] days={"T2","T3","T4","T5","T6","T7","CN"};
        int left=15, top=70, cellW=Math.Max(25,(Width-30)/7), cellH=26;
        for(int i=0;i<7;i++) g.DrawString(days[i],normal,soft,left+i*cellW+4,top);
        var first=new DateTime(SelectedDate.Year,SelectedDate.Month,1);
        int offset=((int)first.DayOfWeek+6)%7;
        int count=DateTime.DaysInMonth(first.Year,first.Month);
        for(int d=1;d<=count;d++){
            int pos=offset+d-1,row=pos/7,col=pos%7; int x=left+col*cellW,y=top+24+row*cellH;
            if(d==SelectedDate.Day){using var sel=new SolidBrush(Color.FromArgb(73,188,83));g.FillEllipse(sel,x,y-2,23,23);}
            g.DrawString(d.ToString(),normal,white,x+4,y);
        }
    }
}

public sealed class FieldCardControl : RoundedPanel
{
    private readonly Label _name = new();
    private readonly Label _type = new();
    private readonly Label _location = new();
    private readonly Label _price = new();
    private readonly RoundedButton _book = new();
    private readonly Panel _visual = new();
    public event EventHandler? BookClicked;

    public FieldCardControl(string name, string type, string location, decimal price)
    {
        Width = 260; Height = 190; Radius = 13; BorderColor = Color.FromArgb(219,232,239); Margin = new Padding(5);
        _visual.Dock = DockStyle.Top; _visual.Height = 78; _visual.BackColor = Color.FromArgb(12,72,63);
        _visual.Paint += DrawField;
        _name.Text=name; _name.Font=new Font("Segoe UI Semibold",10.5F,FontStyle.Bold); _name.ForeColor=AppTheme.Text; _name.AutoSize=true; _name.Location=new Point(12,88);
        _type.Text=type; _type.Font=new Font("Segoe UI",8F); _type.ForeColor=AppTheme.Muted; _type.AutoSize=true; _type.Location=new Point(12,112);
        _location.Text="⌖ "+location; _location.Font=new Font("Segoe UI",7.6F); _location.ForeColor=AppTheme.Muted; _location.AutoSize=false; _location.Width=150; _location.Height=18; _location.Location=new Point(12,132);
        _price.Text=price.ToString("N0")+"đ/h"; _price.Font=new Font("Segoe UI Semibold",9.5F,FontStyle.Bold); _price.ForeColor=Color.FromArgb(0,132,115); _price.AutoSize=true; _price.Location=new Point(12,159);
        _book.Text="Đặt ngay"; _book.Size=new Size(80,29); _book.Location=new Point(166,153); _book.BackColor=AppTheme.Accent; _book.ForeColor=Color.White; _book.Font=new Font("Segoe UI Semibold",7.8F,FontStyle.Bold); _book.Radius=14; _book.HoverColor=AppTheme.AccentDark; _book.Click += (_,e)=>BookClicked?.Invoke(this,e);
        Controls.AddRange(new Control[]{_visual,_name,_type,_location,_price,_book});
        Resize += (_,__)=>_book.Left=Width-_book.Width-12;
    }
    private void DrawField(object? sender, PaintEventArgs e)
    {
        var g=e.Graphics;g.SmoothingMode=SmoothingMode.AntiAlias;
        using var bg=new LinearGradientBrush(_visual.ClientRectangle,Color.FromArgb(24,132,83),Color.FromArgb(6,63,65),0f);g.FillRectangle(bg,_visual.ClientRectangle);
        using var p=new Pen(Color.FromArgb(145,245,250,235),1.2f);
        var r=new Rectangle(12,10,Math.Max(20,_visual.Width-24),Math.Max(20,_visual.Height-20));g.DrawRectangle(p,r);g.DrawLine(p,_visual.Width/2,10,_visual.Width/2,_visual.Height-10);g.DrawEllipse(p,_visual.Width/2-14,_visual.Height/2-14,28,28);
        using var glow=new SolidBrush(Color.FromArgb(55,255,255,255));g.FillEllipse(glow,_visual.Width-52,12,34,34);
        using var f=new Font("Segoe UI Symbol",14F,FontStyle.Bold);using var b=new SolidBrush(Color.White);g.DrawString("⚽",f,b,_visual.Width-50,13);
        using var star=new Font("Segoe UI",7F,FontStyle.Bold);using var yellow=new SolidBrush(Color.FromArgb(255,218,68));g.DrawString("★ 4.8",star,yellow,12,_visual.Height-23);
    }
}

public sealed class CustomerBannerPanel : RoundedPanel
{
    public CustomerBannerPanel()
    {
        Radius=15; BorderThickness=0; DoubleBuffered=true; BackColor=Color.FromArgb(7,64,72);
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g=e.Graphics;g.SmoothingMode=SmoothingMode.AntiAlias;
        using var bg=new LinearGradientBrush(ClientRectangle,Color.FromArgb(5,53,68),Color.FromArgb(17,119,78),0f);g.FillRectangle(bg,ClientRectangle);
        using var turf=new SolidBrush(Color.FromArgb(45,41,190,92));g.FillEllipse(turf,Width-410,Height-155,510,230);
        using var line=new Pen(Color.FromArgb(80,230,255,240),2f);g.DrawArc(line,Width-300,Height-118,220,150,190,160);g.DrawLine(line,Width-260,Height-22,Width-30,Height-22);
        using var ball=new Font("Segoe UI Symbol",42F,FontStyle.Bold);using var white=new SolidBrush(Color.White);g.DrawString("⚽",ball,white,Width-125,Height-95);
        using var t1=new Font("Segoe UI Semibold",18F,FontStyle.Bold);using var t2=new Font("Segoe UI Semibold",20F,FontStyle.Bold);using var sub=new Font("Segoe UI",8.5F);using var green=new SolidBrush(Color.FromArgb(38,229,128));using var soft=new SolidBrush(Color.FromArgb(225,241,246));
        g.DrawString("Sân thể thao chất lượng",t1,white,28,23);g.DrawString("Trải nghiệm tuyệt vời!",t2,green,28,51);g.DrawString("Đặt sân dễ dàng · Thanh toán nhanh · Nhiều ưu đãi hấp dẫn",sub,soft,30,88);
        using var btn=new SolidBrush(Color.FromArgb(27,202,117));using var path=RoundedPanel.CreateRoundPath(new Rectangle(30,112,125,34),17);g.FillPath(btn,path);using var bf=new Font("Segoe UI Semibold",8.5F,FontStyle.Bold);g.DrawString("Đặt sân ngay  →",bf,white,47,121);
    }
}
