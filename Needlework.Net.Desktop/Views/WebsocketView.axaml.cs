using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using CommunityToolkit.Mvvm.Messaging;
using Needlework.Net.Desktop.Extensions;
using Needlework.Net.Desktop.Messages;
using Needlework.Net.Desktop.ViewModels;
using SukiUI;
using TextMateSharp.Grammars;

namespace Needlework.Net.Desktop.Views;

public partial class WebsocketView : UserControl, IRecipient<ResponseUpdatedMessage>
{
    private TextEditor? _responseEditor;

    public WebsocketView()
    {
        InitializeComponent();
    }

    public void Receive(ResponseUpdatedMessage message)
    {
        _responseEditor!.Text = message.Value;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var vm = (WebsocketViewModel)DataContext!;
        var viewer = this.FindControl<ListBox>("EventViewer");
        viewer!.PropertyChanged += (s, e) => { if (vm.IsTail) viewer.ScrollIntoView(vm.EventLog.Count - 1); };

        _responseEditor = this.FindControl<TextEditor>("ResponseEditor");
        _responseEditor?.ApplyJsonEditorSettings();

        WeakReferenceMessenger.Default.Register(this, nameof(WebsocketViewModel));

        OnBaseThemeChanged(Application.Current!.ActualThemeVariant);
        SukiTheme.GetInstance().OnBaseThemeChanged += OnBaseThemeChanged;
    }

    private void OnBaseThemeChanged(ThemeVariant currentTheme)
    {

        var registryOptions = new RegistryOptions(
            currentTheme == ThemeVariant.Dark ? ThemeName.DarkPlus : ThemeName.LightPlus);

        var responseTmi = _responseEditor.InstallTextMate(registryOptions);
        responseTmi.SetGrammar(registryOptions.GetScopeByLanguageId(registryOptions
            .GetLanguageByExtension(".json").Id));
    }
}