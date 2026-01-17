namespace QrCodeGenerator.Messages;

public class SelectedColorChangedMessage(Color color, QrCodeColor qrCodeColor)
{
    public Color Color { get; } = color;
    public QrCodeColor QrCodeColor { get; } = qrCodeColor;
}
