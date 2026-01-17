namespace QrCodeGenerator.Models;

public class MailGenerator : IGenerator
{
    public static MailGenerator Instance => field ??= new();

    public string Receiver { get; set; } = string.Empty;
    
    public string Subject { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
    
    private MailGenerator()
    {
    }
    
    public string? GeneratePayload()
    {
        return new PayloadGenerator.Mail(Receiver, Subject, Message).ToString();
    }
}