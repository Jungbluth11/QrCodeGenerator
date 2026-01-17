namespace QrCodeGenerator.Models;

public class BookmarkGenerator : IGenerator
{
    public static BookmarkGenerator Instance => field ??= new();
    public string? Url { get; set; } = null;
    public string Title { get; set; } = string.Empty;

    private BookmarkGenerator()
    {
    }

    public string? GeneratePayload()
    {
        return Url == null
            ? null
            : new PayloadGenerator.Bookmark(Url, string.IsNullOrWhiteSpace(Title) ? Url : Title).ToString();
    }

    public string GetImageText()
    {
        string? imagetext = string.IsNullOrWhiteSpace(Title) ? Url : Title;
        return imagetext ?? string.Empty;
    }
}