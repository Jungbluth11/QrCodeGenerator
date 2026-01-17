namespace QrCodeGenerator.Models;

public class MmsGenerator : IGenerator
{
    public static MmsGenerator Instance => field ??= new();

    public string? PhoneNumber { get; set; } = null;
    public string Message { get; set; } = string.Empty;

    private MmsGenerator()
    {
    }

    public string? GeneratePayload()
    {
        return PhoneNumber == null
            ? null 
            : new PayloadGenerator.MMS(PhoneNumber, Message).ToString();
    }

}