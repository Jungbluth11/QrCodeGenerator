namespace QrCodeGenerator.Models;

public class CupsPrintService : IPrintService
{
   public IReadOnlyList<string> GetPrinters()
    {
        ProcessStartInfo psi = new()
        {
            FileName = "lpstat",
            ArgumentList = { "-e" },
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using Process? p = Process.Start(psi);
        string output = p!.StandardOutput.ReadToEnd();
        p.WaitForExit();

        return p.ExitCode != 0
            ? throw new InvalidOperationException(p.StandardError.ReadToEnd())
            : output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    }

    public void PrintPdf(string printer, string pdfPath, int copies = 1)
    {
        if (!File.Exists(pdfPath))
        {
            throw new FileNotFoundException(pdfPath);
        }

        ProcessStartInfo psi = new()
        {
            FileName = "lp",
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        psi.ArgumentList.Add("-d");
        psi.ArgumentList.Add(printer);

        psi.ArgumentList.Add("-n");
        psi.ArgumentList.Add(copies.ToString());


        psi.ArgumentList.Add(pdfPath);

        using Process? p = Process.Start(psi);
        string output = p!.StandardOutput.ReadToEnd();
        p.WaitForExit();

        if (p.ExitCode != 0)
        {
            throw new InvalidOperationException(p.StandardError.ReadToEnd());
        }
    }
}