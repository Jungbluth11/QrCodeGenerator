namespace QrCodeGenerator.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void GroupBoxTheme_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!IsLoaded)
        {
            return;
        }

        string theme = (e.AddedItems[0] as ComboBoxItem)!.Tag.ToString()!;
        Core.Instance.Theme = theme;

        (XamlRoot!.Content as FrameworkElement)!.RequestedTheme = theme switch
        {
            "Light" => ElementTheme.Light,
            "Dark" => ElementTheme.Dark,
            _ => ElementTheme.Default
        };
    }
}
