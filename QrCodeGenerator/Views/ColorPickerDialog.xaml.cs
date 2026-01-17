using Microsoft.UI;

namespace QrCodeGenerator.Views;

public sealed partial class ColorPickerDialog : ContentDialog
{
    public QrCodeColor QrCodeColor { get; init; }
    public Color SelectedColor { get; init; } = Colors.Green;

    public ColorPickerDialog()
    {
        InitializeComponent();
    }

    private void ColorPickerDialog_OnLoaded(object sender, RoutedEventArgs e)
    {
        ColorPickerDialogViewModel viewModel = (DataContext as ColorPickerDialogViewModel)!;
        viewModel.QrCodeColor = QrCodeColor;
        viewModel.SelectedColor = SelectedColor;
    }
}
