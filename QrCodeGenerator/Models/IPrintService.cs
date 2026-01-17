namespace QrCodeGenerator.Models;

public interface IPrintService
{
    public  IReadOnlyList<string> GetPrinters();
    public void PrintPdf(string printer, string pdfPath, int copies);
}
