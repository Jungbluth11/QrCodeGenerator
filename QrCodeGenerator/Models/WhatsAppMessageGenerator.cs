namespace QrCodeGenerator.Models;

public class WhatsAppMessageGenerator : IGenerator
{
    public static WhatsAppMessageGenerator Instance => field ??= new();

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Message { get; set; } = null;

    private WhatsAppMessageGenerator()
    {
    }

    public string? GeneratePayload()
    {
        if (Message == null)
        {
            return null;
        }

        PayloadGenerator.WhatsAppMessage payload = string.IsNullOrWhiteSpace(PhoneNumber)
            ? new(Message)
            : new(PhoneNumber, Message);

        return payload.ToString();
    }
}