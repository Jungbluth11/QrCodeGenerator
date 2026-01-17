namespace QrCodeGenerator.ViewModels;

public partial class PrintQrCodeDialogViewModel : ObservableObject
{
    private readonly PrintQrCode _printQrCode = new();
    [ObservableProperty] private string _currentSelectedPrinter;
    [ObservableProperty] private int _copies = 1;

    public string DialogTitle => Core.StringLocalizer["StringPrint"];
    public string BtnPrintText => Core.StringLocalizer["StringPrint"];
    public string BtnCancalText => Core.StringLocalizer["StringCancel"];

    public IReadOnlyList<string> Printers  => _printQrCode.Printers;

    public PrintQrCodeDialogViewModel()
    {
        _currentSelectedPrinter = Printers[0];
    }

    [RelayCommand]
    private void Print()
    {
        _printQrCode.Print(CurrentSelectedPrinter, Copies);
    }
}