using VectSharp;
using VectSharp.PDF;

namespace QrCodeGenerator.Models;

public class PrintQrCode
{
    private readonly IPrintService _printService;
    public IReadOnlyList<string> Printers => _printService.GetPrinters();

    public PrintQrCode()
    {
        if (OperatingSystem.IsWindows())
        {
            _printService = new WindowsPrintService();
        }
        else
        {
            _printService = new CupsPrintService();
        }
    }

    public void Print(string printer, int copies)
    {
        string fileName = Path.GetTempFileName().Replace(".tmp",".pdf");

        Document pdfDocument = new();
        pdfDocument.Pages.Add(Core.Instance.QrCodeSvg);
        pdfDocument.SaveAsPDF(fileName);

        _printService.PrintPdf(printer, fileName, copies);
    }
}