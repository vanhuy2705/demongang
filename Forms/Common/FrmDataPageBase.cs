using QuanLyThueSanTheThao.Helpers;

namespace QuanLyThueSanTheThao.Forms.Common;

public class FrmDataPageBase : Form
{
    protected readonly Panel Header = new();
    protected readonly Label TitleLabel = new();
    protected readonly TextBox SearchBox = new();
    protected readonly Button RefreshButton = new();
    protected readonly DataGridView Grid = new();

    protected FrmDataPageBase(string title)
    {
        BackColor = AppTheme.Background;
        Padding = new Padding(0);

        Header.Dock = DockStyle.Top;
        Header.Height = 68;
        Header.BackColor = Color.White;
        Header.Padding = new Padding(14);

        TitleLabel.Text = title;
        TitleLabel.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
        TitleLabel.ForeColor = AppTheme.Text;
        TitleLabel.AutoSize = true;
        TitleLabel.Location = new Point(14, 18);

        SearchBox.PlaceholderText = "Tìm kiếm...";
        SearchBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        SearchBox.Size = new Size(250, 30);
        SearchBox.Location = new Point(600, 18);

        RefreshButton.Text = "Làm mới";
        RefreshButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        RefreshButton.Size = new Size(90, 32);
        RefreshButton.Location = new Point(860, 17);
        AppTheme.StyleSecondary(RefreshButton);

        Header.Controls.AddRange(new Control[]{TitleLabel,SearchBox,RefreshButton});
        Header.Resize += (_,__) =>
        {
            RefreshButton.Left = Header.Width - RefreshButton.Width - 14;
            SearchBox.Left = RefreshButton.Left - SearchBox.Width - 10;
        };

        Grid.Dock = DockStyle.Fill;
        AppTheme.StyleGrid(Grid);
        Controls.Add(Grid);
        Controls.Add(Header);
        RefreshButton.Click += (_,__) => LoadData();
        SearchBox.TextChanged += (_,__) => ApplyFilter();
        Load += (_,__) => LoadData();
    }

    protected virtual void LoadData() { }
    protected virtual void ApplyFilter() { }
}
