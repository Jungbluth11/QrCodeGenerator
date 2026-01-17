namespace QrCodeGenerator.Messages;

public class ShowTextOnImageChangedMessage
{
    public bool ShowTextOnImage { get; }
    public string Text { get; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public ShowTextOnImageChangedMessage(bool showTextOnImage, string text)
    {
        ShowTextOnImage = showTextOnImage;
        Text = text;
    }

    public ShowTextOnImageChangedMessage(string text)
    {
        ShowTextOnImage = true;
        Text = text;
    }
}
