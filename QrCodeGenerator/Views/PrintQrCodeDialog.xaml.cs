namespace QrCodeGenerator.Views;

public sealed partial class PrintQrCodeDialog : ContentDialog
{
    public PrintQrCodeDialog()
    {
        InitializeComponent();
        DataContext = new PrintQrCodeDialogViewModel();
    }

    private void UIElement_OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter)
        {
            XamlRoot!.Content!.Focus(FocusState.Programmatic);
        }
    }

    private void PrintQrCodeDialog_OnLoaded(object sender, RoutedEventArgs e)
    {
    }
}
