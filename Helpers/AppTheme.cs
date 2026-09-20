using System.Drawing.Drawing2D;
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
    public static readonly Color InputBorder = Color.FromArgb(206, 221, 231);
    public static readonly Color RowHover = Color.FromArgb(243, 250, 252);
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
        else RoundPlainButton(b);
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
        else RoundPlainButton(b);
    }

    public static void StyleDanger(Button b)
    {
        b.BackColor = Danger;
        b.ForeColor = Color.White;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.Cursor = Cursors.Hand;
        b.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        RoundPlainButton(b);
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
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 245, 241);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Text;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(235, 245, 241);
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
        // Input đã được bọc trong host bo tròn (TextField/SelectField/DateField) -> giữ nguyên viền host
        if (c is TextBox && c.Parent is TextField) { c.ForeColor = Text; return; }
        if (c is ComboBox && c.Parent is SelectField) { c.ForeColor = Text; return; }
        if (c is DateTimePicker && c.Parent is DateField) { c.ForeColor = Text; return; }

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
        Upgrade(form);
        form.BackColor = role == "Customer" ? BackgroundBlue : Background;
        foreach (Control c in form.Controls) ApplyRecursive(c);
    }

    /// <summary>
    /// Nâng cấp giao diện toàn form: bọc TextBox/ComboBox/DateTimePicker vào host bo tròn,
    /// làm tròn nút thường, beautify lưới (hover + pill trạng thái + định dạng cột).
    /// Idempotent — gọi nhiều lần an toàn.
    /// </summary>
    public static void Upgrade(Form form)
    {
        if (form.Tag is string s && s.Contains("upgraded")) return;
        form.Tag = (form.Tag?.ToString() ?? "").Trim() + " upgraded";
        WrapInputs(form);
        foreach (var c in Descendants(form))
        {
            switch (c)
            {
                case RoundedButton:
                    break;
                case Button b:
                    RoundPlainButton(b);
                    break;
                case DataGridView grid:
                    EnhanceGrid(grid);
                    break;
                case CheckBox chk:
                    chk.Cursor = Cursors.Hand;
                    chk.ForeColor = Text;
                    break;
                case LinkLabel ll:
                    ll.LinkBehavior = LinkBehavior.HoverUnderline;
                    break;
            }
        }
    }

    private static IEnumerable<Control> Descendants(Control root)
    {
        foreach (Control c in root.Controls)
        {
            yield return c;
            foreach (var d in Descendants(c)) yield return d;
        }
    }

    /// <summary>Bo tròn nút Button thường (giữ nguyên kiểu field trong Designer).</summary>
    public static void RoundPlainButton(Button b)
    {
        if (b is RoundedButton || b.Tag as string == "rounded") return;
        b.Tag = "rounded";
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        void Apply() => ApplyRegion(b, b.Height >= 30 ? 10 : 8);
        b.Resize += (_, _) => Apply();
        Apply();
    }

    private static void WrapInputs(Control root)
    {
        foreach (Control child in root.Controls) WrapInputs(child);

        foreach (var tb in root.Controls.OfType<TextBox>().Where(t => t.Parent is not TextField).ToList())
        {
            var host = new TextField(tb)
            {
                Left = tb.Left,
                Top = tb.Top,
                Width = tb.Width,
                Height = tb.Multiline ? tb.Height : Math.Max(36, tb.Height + 6),
                Anchor = tb.Anchor,
                Dock = tb.Dock,
                TabIndex = tb.TabIndex,
                TabStop = tb.TabStop,
                Enabled = tb.Enabled
            };
            var parent = tb.Parent;
            int idx = parent.Controls.GetChildIndex(tb);
            parent.Controls.Add(host);
            parent.Controls.SetChildIndex(host, idx);
        }

        foreach (var cb in root.Controls.OfType<ComboBox>().Where(c => c.Parent is not SelectField).ToList())
        {
            var host = new SelectField(cb)
            {
                Left = cb.Left,
                Top = cb.Top,
                Width = cb.Width,
                Height = Math.Max(36, cb.Height + 8),
                Anchor = cb.Anchor,
                Dock = cb.Dock,
                TabIndex = cb.TabIndex,
                TabStop = cb.TabStop,
                Enabled = cb.Enabled
            };
            var parent = cb.Parent;
            int idx = parent.Controls.GetChildIndex(cb);
            parent.Controls.Add(host);
            parent.Controls.SetChildIndex(host, idx);
        }

        foreach (var dt in root.Controls.OfType<DateTimePicker>().Where(c => c.Parent is not DateField).ToList())
        {
            var host = new DateField(dt)
            {
                Left = dt.Left,
                Top = dt.Top,
                Width = dt.Width,
                Height = Math.Max(36, dt.Height + 10),
                Anchor = dt.Anchor,
                Dock = dt.Dock,
                TabIndex = dt.TabIndex,
                TabStop = dt.TabStop,
                Enabled = dt.Enabled
            };
            var parent = dt.Parent;
            int idx = parent.Controls.GetChildIndex(dt);
            parent.Controls.Add(host);
            parent.Controls.SetChildIndex(host, idx);
        }
    }

    private static void ApplyRegion(Control c, int radius)
    {
        if (c.Width < 8 || c.Height < 8) return;
        using var path = RoundedPanel.CreateRoundPath(new Rectangle(0, 0, c.Width, c.Height), radius);
        c.Region?.Dispose();
        c.Region = new Region(path);
    }

    // ============ GRID: hover + pill trạng thái + định dạng cột ============

    private static readonly (string[] Keys, Color Base)[] StatusMap =
    {
        (new[]{ "Đã thanh toán", "Paid", "Đã xác nhận", "Confirmed", "Hoàn thành", "Completed", "Trống", "Đang hoạt động" }, Success),
        (new[]{ "Chờ xác nhận", "Pending", "Thanh toán một phần", "PartiallyPaid", "Đang thuê", "Chưa kích hoạt" }, Warning),
        (new[]{ "Chưa thanh toán", "Unpaid" }, Danger),
        (new[]{ "Đang sử dụng", "InUse" }, Info),
        (new[]{ "Đã hủy", "Cancelled", "Bảo trì", "Ngừng hoạt động", "Khóa" }, Color.FromArgb(130, 146, 160)),
    };

    public static void EnhanceGrid(DataGridView grid)
    {
        if (grid.Tag as string == "enhanced") return;
        grid.Tag = "enhanced";
        StyleGrid(grid);

        grid.CellMouseEnter += (_, e) =>
        {
            if (e.RowIndex < 0 || e.RowIndex >= grid.Rows.Count) return;
            grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = RowHover;
        };
        grid.CellMouseLeave += (_, e) =>
        {
            if (e.RowIndex < 0 || e.RowIndex >= grid.Rows.Count) return;
            grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
        };
        grid.CellPainting += StatusPillPainting;

        // định dạng cột tiền tệ / ngày
        grid.DataBindingComplete += (_, _) => FormatColumns(grid);
        FormatColumns(grid);
    }

    private static void FormatColumns(DataGridView grid)
    {
        foreach (DataGridViewColumn col in grid.Columns)
        {
            string n = col.Name;
            if (new[] { "Giá/giờ", "Tổng tiền", "Đã trả", "Giá trị", "Giá trị đặt", "Tối đa", "Đơn tối thiểu", "Số tiền", "Giá", "Thành tiền" }.Any(k => n.Contains(k)))
            {
                col.DefaultCellStyle.Format = "N0";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else if (new[] { "Bắt đầu", "Kết thúc" }.Any(k => n.Contains(k)))
            {
                col.DefaultCellStyle.Format = "dd/MM HH:mm";
            }
            else if (n.Contains("Ngày") || n.Contains("Ngay"))
            {
                col.DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }
    }

    private static void StatusPillPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.Value is not string s) return;
        (string[] Keys, Color Base)? match = null;
        foreach (var m in StatusMap)
        {
            if (m.Keys.Contains(s, StringComparer.Ordinal)) { match = m; break; }
        }
        if (match == null) return;

        e.Handled = true;
        e.PaintBackground(e.ClipBounds, false);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        string text = s;
        using var f = new Font("Segoe UI Semibold", 8.2F, FontStyle.Bold);
        var ts = g.MeasureString(text, f);
        int pillW = Math.Min((int)ts.Width + 26, e.CellBounds.Width - 8);
        int pillH = Math.Min(23, e.CellBounds.Height - 8);
        var pill = new Rectangle(
            e.CellBounds.X + Math.Max(6, (e.CellBounds.Width - pillW) / 2),
            e.CellBounds.Y + (e.CellBounds.Height - pillH) / 2,
            pillW, pillH);
        using var path = RoundedPanel.CreateRoundPath(pill, pillH / 2);
        using (var bg = new SolidBrush(Color.FromArgb(30, match.Value.Base))) g.FillPath(bg, path);
        using (var pen = new Pen(Color.FromArgb(60, match.Value.Base), 1f)) g.DrawPath(pen, path);
        using (var dot = new SolidBrush(match.Value.Base))
            g.FillEllipse(dot, pill.X + 9, pill.Y + pillH / 2 - 2, 4f, 4f);
        TextRenderer.DrawText(g, text, f, new Rectangle(pill.X + 17, pill.Y, pillW - 18, pillH),
            ControlPaint.Dark(match.Value.Base), TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
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
