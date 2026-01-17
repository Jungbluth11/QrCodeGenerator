namespace QrCodeGenerator.Models;

public class PhoneNumberGenerator : IGenerator
{
    public int GroupLength { get; set; } = 4;

    public static PhoneNumberGenerator Instance => field ??= new();

    public string? PhoneNumber { get; set; } = null;

    public bool UseGroups { get; set; }

    private PhoneNumberGenerator()
    {
    }

    public string? GeneratePayload()
    {
        return PhoneNumber == null 
            ? null 
            : new PayloadGenerator.PhoneNumber(PhoneNumber).ToString();
    }

    public string GetImageText()
    {
        string? imageText = UseGroups ? Core.FormatText(PhoneNumber ?? string.Empty, GroupLength) : PhoneNumber;

        return imageText ?? string.Empty;
    }
}