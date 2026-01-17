using VectSharp;
using VectSharp.ImageSharpUtils;
using VectSharp.Raster;
using VectSharp.SVG;
using Font = VectSharp.Font;
using FontFamily = VectSharp.FontFamily;
using Graphics = VectSharp.Graphics;
using Page = VectSharp.Page;

namespace QrCodeGenerator.Models;

public class Core
{
    private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

    public Colour BackgroundColor { get; set; } = Colours.White;

    public bool DrawQuietZones { get; set; } = true;

    public QRCodeGenerator.ECCLevel EccLevel { get; set; } = QRCodeGenerator.ECCLevel.Default;

    public static string[] EccLevelStrings =>
    [
        StringLocalizer["StringDefault"],
        "L",
        "M",
        "Q",
        "H"
    ];

    public QRCodeGenerator.EciMode EciMode { get; set; } = QRCodeGenerator.EciMode.Default;

    public static string[] EciModeStrings =>
    [
        StringLocalizer["StringDefault"],
        "Iso8859-1",
        "Iso8859-2",
        "UTF-8"
    ];

    public int DesiredSize { get; set; } = 230;

    public Colour ForegroundColor { get; set; } = Colours.Black;

    public GenerateMode GenerateMode { get; set; } = GenerateMode.OptimalSize;

    public static string[] GenerateModeStrings =>
    [
        StringLocalizer["GenerateModeOptimalSize"],
        StringLocalizer["GenerateModeFixedSize"],
        StringLocalizer["GenerateModeModuleSize"]
    ];

    public string ImageText { get; set; } = string.Empty;

    public static Core Instance => field ??= new();

    public List<string> LoadedGenerators { get; } = [];

    public int ModuleSize { get; set; } = 4;

    public string Payload { get; set; } = string.Empty;

    public QRCodeData? QrCodeData { get; set; }

    public SvgQRCode.SvgLogo? QrCodeLogo { get; set; }

    public Page? QrCodeSvg { get; set; }

    public bool ShowTextOnImage { get; set; }

    public static IStringLocalizer StringLocalizer =>
        (Application.Current as App)!.Host!.Services.GetService<IStringLocalizer>()!;

    public string Theme
    {
        get;
        set
        {
            _localSettings.Values["theme"] = value;
            field = value;
        }
    }

    public bool UseUtf8Bom { get; set; }

    private Core()
    {
        Theme = _localSettings.Values["theme"] as string ?? "System";


        if (_localSettings.Values["loadedGenerators"] is not null)
        {
            LoadedGenerators.AddRange(
                JsonSerializer.Deserialize<string[]>((string)_localSettings.Values["loadedGenerators"])!);
        }
        else
        {
            LoadedGenerators.Add("Text");
            _localSettings.Values["loadedGenerators"] = JsonSerializer.Serialize(LoadedGenerators.ToArray());
        }
    }

    public static string FormatText(string text, int groupLength)
    {
        StringBuilder builder = new(text.Length + text.Length / groupLength);

        for (int i = 0; i < text.Length; i++)
        {
            if (i > 0 && i % groupLength == 0)
            {
                builder.Append(' ');
            }

            builder.Append(text[i]);
        }

        return builder.ToString();
    }

    public int CalculateActualQrSize()
    {
        int moduleCount = 21 + (QrCodeData!.Version - 1) * 4;
        int moduleSize = CalculateQrModuleSize();

        return moduleCount * moduleSize;
    }

    public Task<MemoryStream> GenerateQrCodeAsync()
    {
        if (QrCodeData == null)
        {
            return Task.FromResult(new MemoryStream());
        }

        int pixelsPerModule = ModuleSize;

        if (GenerateMode != GenerateMode.ModuleSize)
        {
            pixelsPerModule = CalculateQrModuleSize();
        }

        SvgQRCode svgQrCode = new(QrCodeData);

        string qrCodeString = svgQrCode.GetGraphic(pixelsPerModule,
            ForegroundColor.ToCSSString(false),
            BackgroundColor.ToCSSString(false),
            DrawQuietZones,
            SvgQRCode.SizingMode.WidthHeightAttribute,
            QrCodeLogo);

        Parser.ParseImageURI = ImageURIParser.Parser(Parser.ParseSVGURI);
        Page qrCodeImage = Parser.FromString(qrCodeString);

        int additonalSpace = 0;
        int fontSize = 0;
        string[] imageTextLines = ImageText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        if (ShowTextOnImage)
        {
            fontSize = (int)Math.Round(qrCodeImage.Width / 10);

            if (fontSize < 5)
            {
                fontSize = 5;
            }

            additonalSpace = fontSize * imageTextLines.Length + imageTextLines.Length * 5;
        }

        double width = GenerateMode == GenerateMode.FixedSize ? DesiredSize : qrCodeImage.Width;
        double height = GenerateMode == GenerateMode.FixedSize ? DesiredSize : qrCodeImage.Height;

        QrCodeSvg = new(width, height + additonalSpace) { Background = BackgroundColor };

        double originX = GenerateMode == GenerateMode.FixedSize ? (DesiredSize - qrCodeImage.Width) / 2 : 0;
        double originY = GenerateMode == GenerateMode.FixedSize ? (DesiredSize - qrCodeImage.Height) / 2 : 0;

        Graphics graphics = QrCodeSvg.Graphics;
        graphics.DrawGraphics(originX, originY, qrCodeImage.Graphics);

        if (ShowTextOnImage)
        {
            Font font = new(FontFamily.ResolveFontFamily(FontFamily.StandardFontFamilies.Helvetica), fontSize);

            double textWidth = imageTextLines.Select(textLine => font.MeasureText(textLine))
                .Select(textSize => textSize.Width)
                .Prepend(0)
                .Max();

            double startPosition = 0;

            if (textWidth < QrCodeSvg.Width)
            {
                startPosition = (QrCodeSvg.Width - textWidth) / 2;
            }

            for (int i = 0, n = 0; i < imageTextLines.Length; i++, n += fontSize + 5)
            {
                graphics.FillText(startPosition, qrCodeImage.Height + n, imageTextLines[i], font, ForegroundColor);
            }
        }

        MemoryStream ms = new();
        QrCodeSvg.SaveAsPNG(ms);

        return Task.FromResult(ms);
    }

    public void GenerateQrCodeData()
    {
        try
        {
            QrCodeData = QRCodeGenerator.GenerateQrCode(Payload, EccLevel, false, UseUtf8Bom, EciMode);
        }
        catch (DataTooLongException)
        {
            WeakReferenceMessenger.Default.Send(new ErrorMessage(StringLocalizer["StringErrorDataTooLong"]));
        }
    }

    public Task<bool> IsPayloadLengthValidAsync(string payload)
    {
        try
        {
            _ = QRCodeGenerator.GenerateQrCode(payload, EccLevel, false, UseUtf8Bom, EciMode);

            return Task.FromResult(true);
        }
        catch (DataTooLongException)
        {
            return Task.FromResult(false);
        }
    }

    public async Task LoadQrCodeLogoAsync(StorageFile file)
    {
        if (file.FileType == ".png")
        {
            await using Stream stream = await file.OpenStreamForReadAsync();
            await using MemoryStream ms = new();
            await stream.CopyToAsync(ms);
            QrCodeLogo = new(ms.ToArray(), 20);
        }
        else
        {
            QrCodeLogo = new(File.ReadAllText(file.Path), 20);
        }
    }

    public void SaveQrCode(StorageFile file)
    {
        if (file.FileType == ".png")
        {
            QrCodeSvg.SaveAsPNG(file.Path);
        }
        else
        {
            QrCodeSvg.SaveAsSVG(file.Path);
        }
    }

    public void ToggleGenerator(string generatorName)
    {
        if (!LoadedGenerators.Remove(generatorName))
        {
            LoadedGenerators.Add(generatorName);
        }

        _localSettings.Values["loadedGenerators"] = JsonSerializer.Serialize(LoadedGenerators.ToArray());
    }

    private int CalculateQrModuleSize()
    {
        int moduleCount = 21 + (QrCodeData!.Version - 1) * 4;

        if (DrawQuietZones)
        {
            moduleCount += 8;
        }

        int moduleSizePx = DesiredSize / moduleCount;

        if (moduleSizePx < 1)
        {
            moduleSizePx = 1;
        }

        return moduleSizePx;
    }
}