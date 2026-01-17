namespace QrCodeGenerator.Models;

public class WindowsPrintService : IPrintService
{
    public IReadOnlyList<string> GetPrinters()
    {
        ProcessStartInfo psi = new()
        {
            FileName = "powershell",
            ArgumentList =
            {
                "-NoProfile",
                "-Command",
                "Get-Printer | Select-Object -ExpandProperty Name"
            },
            RedirectStandardOutput = true
        };

        using Process? p = Process.Start(psi);
        string output = p!.StandardOutput.ReadToEnd();
        p.WaitForExit();

        return output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    }

    public void PrintPdf(string printer, string pdfPath, int copies)
    {
        if (!File.Exists(pdfPath))
        {
            throw new FileNotFoundException(pdfPath);
        }

        //PowerShell workaround for more then one copy
        for (int i = 0; i < copies; i++)
        {

            string command = $@"
        $printer = ""{printer}""
        Start-Process -FilePath ""{pdfPath}"" `
            -Verb PrintTo `
            -ArgumentList ""$printer"" `
            -PassThru |
            ForEach-Object {{ $_.WaitForExit() }}
        ";

            ProcessStartInfo psi = new()
            {
                FileName = "powershell", ArgumentList = {"-NoProfile", "-Command", command}, RedirectStandardError = true
            };

            using Process? p = Process.Start(psi);
            p!.WaitForExit();

            if (p.ExitCode != 0)
            {
                throw new InvalidOperationException(p.StandardError.ReadToEnd());
            }
        }
    }
}