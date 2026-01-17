namespace QrCodeGenerator.Models;

public class SmsGenerator : IGenerator
{
    public static SmsGenerator Instance => field ??= new();

    public string? PhoneNumber { get; set; } = null;

    public string Message { get; set; } = string.Empty;

    private SmsGenerator()
    {
    }

    public string? GeneratePayload()
    {
        return PhoneNumber == null
            ? null
            : new PayloadGenerator.SMS(PhoneNumber, Message).ToString();
    }
}