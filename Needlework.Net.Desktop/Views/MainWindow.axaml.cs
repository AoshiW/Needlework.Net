using FluentAvalonia.UI.Windowing;

namespace Needlework.Net.Desktop.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();

        TitleBar.ExtendsContentIntoTitleBar = true;
    }
}