namespace QrCodeGenerator.ViewModels;

public partial class ColorPickerDialogViewModel : ObservableObject
{
    [ObservableProperty] private Color _selectedColor;

    public string BtnCancalText => Core.StringLocalizer["StringCancel"];
    public QrCodeColor QrCodeColor { get; set; }

    [RelayCommand]
    private void Submit()
    {
        WeakReferenceMessenger.Default.Send(new SelectedColorChangedMessage(SelectedColor, QrCodeColor));
    }
}
