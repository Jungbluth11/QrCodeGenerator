using CommunityToolkit.WinUI.Helpers;

using VectSharp;

namespace QrCodeGenerator.Models;

public static class ColorConverter
{
    public static Colour ConvertToVectSharpColour(Color color) => Colour.FromRgb(color.R, color.G, color.B);

    public static Color ConvertFromVectSharpColour(Colour colour) => colour.ToCSSString(false).ToColor();
}
