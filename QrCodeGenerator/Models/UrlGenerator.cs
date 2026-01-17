namespace QrCodeGenerator.Models;

public class UrlGenerator : IGenerator
{
    public static UrlGenerator Instance => field ??= new();

    public string? Url { get; set; } = null;

    private UrlGenerator()
    {
    }

    public string? GeneratePayload()
    {
        return Url == null ? null : new PayloadGenerator.Url(Url).ToString();
    }
}