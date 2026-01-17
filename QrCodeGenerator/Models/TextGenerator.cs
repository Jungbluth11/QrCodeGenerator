namespace QrCodeGenerator.Models;

public class TextGenerator : IGenerator
{
    private static readonly Regex Whitespace = new(@"\s+");
    public static TextGenerator Instance => field ??= new();
    public bool RemoveWhitespace { get; set; }
    public string Text { get; set; } = string.Empty;

    private TextGenerator()
    {
    }

    public string? GeneratePayload()
    {
        return RemoveWhitespace ? Whitespace.Replace(Text, string.Empty) : Text;
    }
}
