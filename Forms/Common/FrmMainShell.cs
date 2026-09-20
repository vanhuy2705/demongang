using QuanLyThueSanTheThao.Forms.Admin;
using QuanLyThueSanTheThao.Forms.Customer;
using QuanLyThueSanTheThao.Forms.Employee;
using QuanLyThueSanTheThao.Helpers;

namespace QuanLyThueSanTheThao.Forms.Common;

public partial class FrmMainShell : Form
{
    private Form? _current;
    private Button? _activeButton;
    private FlowLayoutPanel _menu = null!;
    private Panel _content = null!;
    private Label _pageTitle = null!;
    private Label _clock = null!;
    private readonly System.Windows.Forms.Timer _clockTimer = new() { Interval = 1000 };
    private readonly List<(RoundedButton Button, MenuDef Definition)> _menuButtons = new();

    public FrmMainShell()
    {
        InitializeComponent();AppTheme.Upgrade(this);
        BuildShell();
        _clockTimer.Tick += (_,__) => UpdateClock();
        _clockTimer.Start();
        UpdateClock();
        Shown += (_,__) => OpenDefault();
        FormClosed += (_,__) => _clockTimer.Stop();
    }

    private record MenuDef(string Text, string Icon, Func<Form> Create);

    private List<MenuDef> GetMenuItems() => SessionContext.Role switch
    {
        "Admin" => new List<MenuDef>
        {
            new("Tổng quan","⌂",()=>new FrmAdminDashboard()),
            new("Loại sân","◇",()=>new FrmFieldTypes()),
            new("Quản lý sân","◎",()=>new FrmFields()),
            new("Khách hàng","♙",()=>new FrmCustomers()),
            new("Đặt sân","▣",()=>new FrmBooking()),
            new("Lịch đặt sân","▦",()=>new FrmBookingSchedule()),
            new("Hóa đơn","▤",()=>new FrmInvoices()),
            new("Voucher","▱",()=>new FrmVouchers()),
            new("Khuyến mãi","✦",()=>new FrmPromotions()),
            new("Tài khoản","▧",()=>new FrmAccounts()),
            new("Nhân viên","♟",()=>new FrmEmployees()),
            new("Thống kê","⌁",()=>new FrmStatistics()),
            new("Cấu hình","⚙",()=>new FrmSettings())
        },
        "Employee" => new List<MenuDef>
        {
            new("Trang chủ","⌂",()=>new FrmEmployeeDashboard()),
            new("Đặt sân","▣",()=>new FrmBooking()),
            new("Lịch đặt sân","▦",()=>new FrmBookingSchedule()),
            new("Hóa đơn","▤",()=>new FrmInvoices()),
            new("Khách hàng","♙",()=>new FrmCustomers()),
            new("Thông tin sân","◎",()=>new FrmFields(true)),
            new("Loại sân","◇",()=>new FrmFieldTypes(true)),
            new("Thống kê","⌁",()=>new FrmStatistics()),
            new("Tài khoản","♙",()=>new FrmEmployeeAccount())
        },
        _ => new List<MenuDef>
        {
            new("Trang chủ","⌂",()=>new FrmCustomerHome()),
            new("Đặt sân","▣",()=>new FrmBooking(customerMode:true)),
            new("Lịch sử đặt sân","▦",()=>new FrmCustomerBookingHistory()),
            new("Hóa đơn của tôi","▤",()=>new FrmCustomerInvoices()),
            new("Voucher của tôi","▱",()=>new FrmCustomerVouchers()),
            new("Thông tin cá nhân","♙",()=>new FrmCustomerProfile())
        }
    };

    private void BuildShell()
    {
        root.Controls.Clear();
        _menuButtons.Clear();
        if (SessionContext.Role == "Admin") BuildAdminShell();
        else BuildSideShell(SessionContext.Role == "Customer");
        foreach (var def in GetMenuItems()) AddMenuButton(def);
        AppTheme.Smooth(this);
    }

    private void BuildAdminShell()
    {
        root.BackColor = AppTheme.Background;

        var top = new Panel
        {
            Dock = DockStyle.Top,
            Height = 78,
            BackColor = Color.White,
            Padding = new Padding(18, 6, 18, 6)
        };
        top.Paint += (_, e) =>
        {
            using var p = new Pen(Color.FromArgb(229, 237, 242));
            e.Graphics.DrawLine(p, 0, top.Height - 1, top.Width, top.Height - 1);
        };

        var brand = new Panel { Dock = DockStyle.Left, Width = 205, BackColor = Color.White };
        var logo = new IconBadge { Glyph = "▦", AccentColor = AppTheme.Accent2, Size = new Size(44,44), Location = new Point(2,10) };
        var brandTitle = new Label { Text = "THUÊ SÂN", AutoSize = true, Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold), ForeColor = Color.FromArgb(22,75,132), Location = new Point(52,8) };
        var brandSub = new Label { Text = "Đặt sân dễ dàng · Chơi hết mình", AutoSize = true, Font = new Font("Segoe UI", 7.2F), ForeColor = AppTheme.Muted, Location = new Point(53,37) };
        brand.Controls.AddRange(new Control[] { logo, brandTitle, brandSub });

        _menu = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            AutoScroll = true,
            BackColor = Color.White,
            Padding = new Padding(2, 0, 2, 0)
        };

        var user = new Panel { Dock = DockStyle.Right, Width = 220, BackColor = Color.White };
        var avatar = new IconBadge { Glyph = "♙", AccentColor = Color.FromArgb(215,228,237), Filled = true, Size = new Size(38,38), Location = new Point(8,11) };
        var name = new Label { Text = SessionContext.FullName, AutoSize = false, Width = 145, Height = 22, Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold), ForeColor = AppTheme.Text, Location = new Point(54,10), TextAlign = ContentAlignment.MiddleLeft };
        var role = new Label { Text = "Quản trị viên ⌄", AutoSize = false, Width = 145, Height = 20, Font = new Font("Segoe UI", 7.8F), ForeColor = AppTheme.Muted, Location = new Point(54,31), TextAlign = ContentAlignment.MiddleLeft };
        var logout = new RoundedButton { Text = "Đăng xuất", Width = 78, Height = 25, Radius = 8, BackColor = Color.FromArgb(241,248,250), ForeColor = AppTheme.Text, Font = new Font("Segoe UI",7.5F), Location = new Point(128,51) };
        logout.Click += (_,__) => Close();
        user.Controls.AddRange(new Control[] { avatar, name, role, logout });

        top.Controls.Add(_menu);
        top.Controls.Add(user);
        top.Controls.Add(brand);

        _content = new Panel { Dock = DockStyle.Fill, BackColor = AppTheme.Background, Padding = new Padding(16, 12, 16, 16) };
        _pageTitle = new Label { Visible = false };
        _clock = new Label { Visible = false };

        var accentStrip = new Panel { Dock = DockStyle.Top, Height = 3, BackColor = AppTheme.Accent };
        accentStrip.Paint += (_, e) =>
        {
            using var br = new System.Drawing.Drawing2D.LinearGradientBrush(accentStrip.ClientRectangle, AppTheme.Accent, AppTheme.Accent2, 0f);
            e.Graphics.FillRectangle(br, accentStrip.ClientRectangle);
        };

        root.Controls.Add(_content);
        root.Controls.Add(top);
        root.Controls.Add(accentStrip);
    }

    private void BuildSideShell(bool customer)
    {
        root.BackColor = customer ? AppTheme.BackgroundBlue : Color.FromArgb(238, 248, 252);
        var sidebar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 232,
            BackColor = customer ? Color.White : AppTheme.Sidebar,
            Padding = new Padding(12, 10, 12, 12)
        };
        if (customer)
        {
            sidebar.Paint += (_,e) => { using var p = new Pen(Color.FromArgb(224,234,239)); e.Graphics.DrawLine(p, sidebar.Width-1,0,sidebar.Width-1,sidebar.Height); };
        }

        var brand = new Panel { Dock = DockStyle.Top, Height = 62, BackColor = sidebar.BackColor };
        var brandIcon = new IconBadge { Glyph = "⚽", AccentColor = AppTheme.Accent, Size = new Size(40,40), Location = new Point(2,5) };
        var brandText = new Label { Text = "SportField", AutoSize = true, Font = new Font("Segoe UI Semibold",15F,FontStyle.Bold), ForeColor = customer ? AppTheme.Text : Color.White, Location = new Point(48,6) };
        var fieldStart = brandText.Text.IndexOf("Field", StringComparison.Ordinal);
        var brandSub = new Label { Text = "Đặt sân dễ dàng · Chơi hết mình", AutoSize = true, Font = new Font("Segoe UI",6.8F), ForeColor = customer ? AppTheme.Muted : Color.FromArgb(178,209,215), Location = new Point(49,34) };
        brand.Controls.AddRange(new Control[] { brandIcon, brandText, brandSub });

        var profile = new Panel { Dock = DockStyle.Top, Height = 66, BackColor = sidebar.BackColor, Padding = new Padding(0,4,0,4) };
        var av = new IconBadge { Glyph = "♙", AccentColor = customer ? Color.FromArgb(205,226,235) : Color.FromArgb(221,230,234), Size = new Size(40,40), Location = new Point(4,8) };
        var n = new Label { Text = SessionContext.FullName, AutoSize = false, Width = 155, Height = 22, Font = new Font("Segoe UI Semibold",9F,FontStyle.Bold), ForeColor = customer ? AppTheme.Text : Color.White, Location = new Point(52,7), TextAlign = ContentAlignment.MiddleLeft };
        var r = new Label { Text = customer ? "Khách hàng" : "Nhân viên", AutoSize = false, Width = 155, Height = 20, Font = new Font("Segoe UI",8F), ForeColor = customer ? AppTheme.Muted : Color.FromArgb(166,201,208), Location = new Point(52,28), TextAlign = ContentAlignment.MiddleLeft };
        profile.Controls.AddRange(new Control[] { av,n,r });

        _menu = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            BackColor = sidebar.BackColor,
            Padding = new Padding(0, 4, 0, 4)
        };

        var logout = new RoundedButton
        {
            Dock = DockStyle.Bottom,
            Height = 44,
            Text = "↪   Đăng xuất",
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10,0,0,0),
            BackColor = sidebar.BackColor,
            ForeColor = customer ? AppTheme.Text : Color.White,
            HoverColor = customer ? Color.FromArgb(238,247,248) : Color.FromArgb(16,51,61),
            Font = new Font("Segoe UI",9F)
        };
        logout.Click += (_,__) => Close();

        sidebar.Controls.Add(_menu);
        sidebar.Controls.Add(logout);
        sidebar.Controls.Add(profile);
        sidebar.Controls.Add(brand);

        var main = new Panel { Dock = DockStyle.Fill, BackColor = root.BackColor };
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = customer ? 60 : 50,
            BackColor = customer ? Color.FromArgb(239,249,253) : Color.FromArgb(12,60,80),
            Padding = new Padding(14, 8, 16, 8)
        };
        if (!customer)
        {
            header.Paint += (_,e) =>
            {
                using var br = new System.Drawing.Drawing2D.LinearGradientBrush(header.ClientRectangle, Color.FromArgb(12,63,82), Color.FromArgb(31,107,145), 0f);
                e.Graphics.FillRectangle(br, header.ClientRectangle);
                using var turf = new SolidBrush(Color.FromArgb(30, AppTheme.Accent));
                e.Graphics.FillEllipse(turf, header.Width-360, -45, 340, 130);
            };
        }

        _pageTitle = new Label
        {
            Text = customer ? "" : "SportField",
            AutoSize = true,
            Font = new Font("Segoe UI Semibold",10F,FontStyle.Bold),
            ForeColor = customer ? AppTheme.Text : Color.White,
            Location = new Point(16,17)
        };
        _clock = new Label
        {
            AutoSize = false,
            Width = 235,
            Height = 36,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = customer ? AppTheme.Text : Color.White,
            Font = new Font("Segoe UI Semibold",8.2F),
            Location = new Point(900,6)
        };
        var notify = new IconBadge { Glyph = "!", AccentColor = customer ? AppTheme.Info : Color.FromArgb(44,84,103), Size = new Size(32,32), Anchor = AnchorStyles.Top|AnchorStyles.Right, Location = new Point(1160,8) };
        var headerAvatar = new IconBadge { Glyph = "♙", AccentColor = customer ? Color.FromArgb(221,236,244) : Color.FromArgb(234,240,243), Size = new Size(32,32), Anchor = AnchorStyles.Top|AnchorStyles.Right, Location = new Point(1200,8) };
        header.Controls.AddRange(new Control[] { _pageTitle,_clock,notify,headerAvatar });
        header.Resize += (_,__) =>
        {
            headerAvatar.Left = header.Width - 48;
            notify.Left = headerAvatar.Left - 42;
            _clock.Left = notify.Left - _clock.Width - 12;
        };

        _content = new Panel { Dock = DockStyle.Fill, BackColor = root.BackColor, Padding = customer ? new Padding(14,8,14,14) : new Padding(12) };
        main.Controls.Add(_content);
        main.Controls.Add(header);

        root.Controls.Add(main);
        root.Controls.Add(sidebar);
    }

    private void AddMenuButton(MenuDef def)
    {
        bool admin = SessionContext.Role == "Admin";
        bool customer = SessionContext.Role == "Customer";
        var btn = new RoundedButton
        {
            Text = admin ? $"{def.Icon}\n{def.Text}" : $"{def.Icon}    {def.Text}",
            TextAlign = admin ? ContentAlignment.MiddleCenter : ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI Semibold", admin ? 7.8F : 9.2F, FontStyle.Regular),
            ForeColor = admin ? Color.FromArgb(27,79,136) : (customer ? AppTheme.Text : Color.White),
            BackColor = admin ? Color.White : (customer ? Color.White : AppTheme.Sidebar),
            HoverColor = admin ? Color.FromArgb(234,247,240) : (customer ? Color.FromArgb(237,249,245) : Color.FromArgb(14,58,65)),
            Radius = admin ? 8 : 10,
            Cursor = Cursors.Hand,
            Margin = admin ? new Padding(2, 0, 2, 0) : new Padding(0, 2, 0, 2),
            Width = admin ? Math.Max(68, Math.Min(90, TextRenderer.MeasureText(def.Text, new Font("Segoe UI",7.5F)).Width + 18)) : 204,
            Height = admin ? 62 : 38,
            Padding = admin ? new Padding(0) : new Padding(10,0,0,0)
        };
        btn.Click += (_,__) => { Activate(btn); OpenChild(def.Text, def.Create()); };
        _menu.Controls.Add(btn);
        _menuButtons.Add((btn, def));
    }

    private void Activate(Button button)
    {
        if (_activeButton is RoundedButton prev)
        {
            bool admin = SessionContext.Role == "Admin";
            bool customer = SessionContext.Role == "Customer";
            prev.BackColor = admin ? Color.White : (customer ? Color.White : AppTheme.Sidebar);
            prev.ForeColor = admin ? Color.FromArgb(27,79,136) : (customer ? AppTheme.Text : Color.White);
            prev.Font = new Font("Segoe UI Semibold", admin ? 7.8F : 9.2F, FontStyle.Regular);
            prev.IndicatorBar = null;
            prev.Invalidate();
        }
        _activeButton = button;
        bool isAdmin = SessionContext.Role == "Admin";
        if (button is RoundedButton rb)
        {
            Fx.StopColor(rb);
            if (isAdmin)
            {
                rb.BackColor = Color.FromArgb(234, 247, 240);
                rb.ForeColor = AppTheme.AccentDark;
                rb.IndicatorBar = AppTheme.Accent;
                rb.IndicatorBottom = true;
            }
            else
            {
                rb.BackColor = SessionContext.Role == "Customer" ? Color.FromArgb(16, 172, 99) : Color.FromArgb(19, 156, 101);
                rb.ForeColor = Color.White;
                rb.IndicatorBar = Color.FromArgb(120, 255, 255, 255);
                rb.IndicatorBottom = false;
            }
            rb.SyncBaseColor();
            rb.Invalidate();
        }
        button.Font = new Font("Segoe UI Semibold", isAdmin ? 7.8F : 9.2F, FontStyle.Bold);
    }

    private void OpenDefault()
    {
        var first = _menuButtons.FirstOrDefault();
        if (first.Button == null) return;
        Activate(first.Button);
        OpenChild(first.Definition.Text, first.Definition.Create());
    }

    private void OpenChild(string title, Form form)
    {
        _current?.Close();
        _current = form;
        _pageTitle.Text = title;
        form.TopLevel = false;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;
        AppTheme.ApplyToForm(form, SessionContext.Role);
        _content.SuspendLayout();
        _content.Controls.Clear();
        _content.Controls.Add(form);
        _content.ResumeLayout();
        form.Show();
        Fx.Settle(form);
    }

    private void UpdateClock()
    {
        if (_clock == null) return;
        _clock.Text = SessionContext.Role == "Customer"
            ? DateTime.Now.ToString("dd/MM/yyyy   HH:mm")
            : DateTime.Now.ToString("dddd, dd/MM/yyyy\nHH:mm tt");
    }
}
