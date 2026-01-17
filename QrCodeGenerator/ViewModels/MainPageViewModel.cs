namespace QrCodeGenerator.ViewModels;

public sealed partial class MainPageViewModel : ObservableObject,
    IRecipient<LoadedGeneratorsChangedMessage>,
    IRecipient<QrCodePayloadChangedMessage>,
    IRecipient<ShowTextOnImageChangedMessage>,
    IRecipient<SelectedColorChangedMessage>
{
    private readonly Core _core = Core.Instance;
    [ObservableProperty] private BitmapImage _qrCodeImage = new();
    [ObservableProperty] private bool _drawQuietZones = true;
    [ObservableProperty] private bool _isDesiredSizeEnabled = true;
    [ObservableProperty] private bool _isModuleSizeEnabled;
    [ObservableProperty] private bool _isUseUtf8BomEnabled;
    [ObservableProperty] private bool _useUtf8Bom;
    [ObservableProperty] private Color _backgroundColor;
    [ObservableProperty] private Color _foregroundColor;
    [ObservableProperty] private int _actualSize;
    [ObservableProperty] private int _desiredSize;
    [ObservableProperty] private int _moduleSize;
    [ObservableProperty] private int _qrCodeImageHeight;
    [ObservableProperty] private int _qrCodeImageWidth;
    [ObservableProperty] private string _currentEccLevel;
    [ObservableProperty] private string _currentEciMode;
    [ObservableProperty] private string _currentGenerateMode;
    [ObservableProperty] private string _qrCodeLogoPath = string.Empty;
    public string[] EccLevelStrings => Core.EccLevelStrings;
    public string[] EciModeStrings => Core.EciModeStrings;
    public string[] GenerateModeStrings => Core.GenerateModeStrings;
    public ObservableCollection<NavigationViewMenuItem> MenuItems { get; } = [];
    
    public MainPageViewModel()
    {
        _desiredSize = _core.DesiredSize;
        _actualSize = 0;
        _moduleSize = _core.ModuleSize;
        _currentGenerateMode = GenerateModeStrings[0];
        _currentEccLevel = EccLevelStrings[0];
        _currentEciMode = EciModeStrings[0];
        _foregroundColor = ColorConverter.ConvertFromVectSharpColour(_core.ForegroundColor);
        _backgroundColor = ColorConverter.ConvertFromVectSharpColour(_core.BackgroundColor);

        foreach (string generator in _core.LoadedGenerators)
        {
            _ = AddMenuItem(generator);
        }

        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    public void Receive(LoadedGeneratorsChangedMessage message)
    {
        NavigationViewMenuItem[] menuItems =
        [
            ..from item in MenuItems
            where item.Name == Core.StringLocalizer[$"Menu{message.Value}.Content"]
            select item
        ];

        if (menuItems.Any())
        {
            MenuItems.Remove(menuItems[0]);
        }
        else
        {
            _ = AddMenuItem(message.Value);
        }
    }

    public void Receive(QrCodePayloadChangedMessage message)
    {
        if (message.Value == null)
        {
            QrCodeImage = new();

            return;
        }

        _core.Payload = message.Value;
        _core.GenerateQrCodeData();
        _ = GenerateImageAsync();

        if (_core.GenerateMode == GenerateMode.OptimalSize)
        {
            ActualSize = _core.CalculateActualQrSize();
        }
    }

    public void Receive(SelectedColorChangedMessage message)
    {
        if (message.QrCodeColor == QrCodeColor.ForegroundColor)
        {
            ForegroundColor = message.Color;
        }
        else
        {
            BackgroundColor = message.Color;
        }
    }

    public void Receive(ShowTextOnImageChangedMessage message)
    {
        _core.ShowTextOnImage = message.ShowTextOnImage;
        _core.ImageText = message.Text;
        _ = GenerateImageAsync();
    }

    public async Task LoadQrCodeLogoAsync(StorageFile file)
    {
        QrCodeLogoPath = file.Path;
        await _core.LoadQrCodeLogoAsync(file);
        _ = GenerateImageAsync();
    }

    private Task AddMenuItem(string name)
    {
        switch (name)
        {
            case "Text":
                MenuItems.Add(new(Core.StringLocalizer["MenuText.Content"],
                    WebUtility.HtmlDecode("&#xE8D2;"),
                    typeof(TextGeneratorPage)));

                break;
            case "WiFi":
                MenuItems.Add(new(Core.StringLocalizer["MenuWiFi.Content"],
                    WebUtility.HtmlDecode("&#xE701;"),
                    typeof(WifiGeneratorPage)));

                break;
            case "URL":
                MenuItems.Add(new(Core.StringLocalizer["MenuURL.Content"],
                    WebUtility.HtmlDecode("&#xE71B;"),
                    typeof(UrlGeneratorPage)));

                break;
            case "Bookmark":
                MenuItems.Add(new(Core.StringLocalizer["MenuBookmark.Content"],
                    WebUtility.HtmlDecode("&#xE735;"),
                    typeof(BookmarkGeneratorPage)));

                break;
            case "Mail":
                MenuItems.Add(new(Core.StringLocalizer["MenuMail.Content"],
                    WebUtility.HtmlDecode("&#xE715;"),
                    typeof(MailGeneratorPage)));

                break;
            case "SMS":
                MenuItems.Add(new(Core.StringLocalizer["MenuSMS.Content"],
                    WebUtility.HtmlDecode("&#xE8BD;"),
                    typeof(SmsGeneratorPage)));

                break;
            case "MMS":
                MenuItems.Add(new(Core.StringLocalizer["MenuMMS.Content"],
                    WebUtility.HtmlDecode("&#xEB9F;"),
                    typeof(MmsGeneratorPage)));

                break;
            case "Geolocation":
                MenuItems.Add(new(Core.StringLocalizer["MenuGeolocation.Content"],
                    WebUtility.HtmlDecode("&#xE707;"),
                    typeof(GeolocationGeneratorPage)));

                break;
            case "PhoneNumber":
                MenuItems.Add(new(Core.StringLocalizer["MenuPhoneNumber.Content"],
                    WebUtility.HtmlDecode("&#xE717;"),
                    typeof(PhoneNumberGeneratorPage)));

                break;
            case "WhatsAppMessage":
                MenuItems.Add(new(Core.StringLocalizer["MenuWhatsAppMessage.Content"],
                    WebUtility.HtmlDecode("&#xE8F2;"),
                    typeof(WhatsAppMessageGeneratorPage)));

                break;
            case "ContactData":
                MenuItems.Add(new(Core.StringLocalizer["MenuContactData.Content"],
                    WebUtility.HtmlDecode("&#xE77B;"),
                    typeof(ContactDataGeneratorPage)));

                break;
            case "CryptoCurrency":
                MenuItems.Add(new(Core.StringLocalizer["MenuCryptoCurrency.Content"],
                    WebUtility.HtmlDecode("&#xED14;"),
                    typeof(CryptoCurrencyGeneratorPage)));

                break;
        }

        return Task.CompletedTask;
    }

    private async Task GenerateImageAsync()
    {
        if (string.IsNullOrEmpty(_core.Payload))
        {
            return;
        }

        await using MemoryStream ms = await _core.GenerateQrCodeAsync();
        await QrCodeImage.SetSourceAsync(ms.AsRandomAccessStream());

        int width = QrCodeImage.PixelWidth;
        int height = QrCodeImage.PixelHeight;

        if (width != QrCodeImageWidth)
        {
            QrCodeImageWidth = width;
        }

        if (height != QrCodeImageHeight)
        {
            QrCodeImageHeight = height;
        }
    }

    partial void OnBackgroundColorChanged(Color value)
    {
        _core.BackgroundColor = ColorConverter.ConvertToVectSharpColour(value);
        _ = GenerateImageAsync();
    }

    partial void OnCurrentEccLevelChanged(string value)
    {
        _core.EccLevel = value switch
        {
            "L" => QRCodeGenerator.ECCLevel.L,
            "M" => QRCodeGenerator.ECCLevel.M,
            "Q" => QRCodeGenerator.ECCLevel.Q,
            _ => QRCodeGenerator.ECCLevel.H
        };

        _core.GenerateQrCodeData();
        _ = GenerateImageAsync();
    }

    partial void OnCurrentEciModeChanged(string value)
    {
        _core.EciMode = value switch
        {
            "Iso8859-1" => QRCodeGenerator.EciMode.Iso8859_1,
            "Iso8859-2" => QRCodeGenerator.EciMode.Iso8859_2,
            "UTF-8" => QRCodeGenerator.EciMode.Utf8,
            _ => QRCodeGenerator.EciMode.Default
        };

        IsUseUtf8BomEnabled = value == "UTF-8";
        _core.GenerateQrCodeData();
        _ = GenerateImageAsync();
    }

    partial void OnCurrentGenerateModeChanged(string value)
    {
        if (value == Core.StringLocalizer["GenerateModeOptimalSize"])
        {
            IsDesiredSizeEnabled = true;
            IsModuleSizeEnabled = false;
            _core.GenerateMode = GenerateMode.OptimalSize;
            ActualSize = _core.CalculateActualQrSize();
        }
        else if (value == Core.StringLocalizer["GenerateModeFixedSize"])
        {
            IsDesiredSizeEnabled = true;
            IsModuleSizeEnabled = false;
            _core.GenerateMode = GenerateMode.FixedSize;
            ActualSize = DesiredSize;
        }
        else
        {
            IsDesiredSizeEnabled = false;
            IsModuleSizeEnabled = true;
            _core.GenerateMode = GenerateMode.ModuleSize;
        }

        _ = GenerateImageAsync();
    }

    partial void OnDesiredSizeChanged(int value)
    {
        if (_core.QrCodeData == null)
        {
            return;
        }

        _core.DesiredSize = value;
        ActualSize = _core.GenerateMode == GenerateMode.FixedSize
            ? DesiredSize
            : _core.CalculateActualQrSize();

        _ = GenerateImageAsync();
    }

    partial void OnDrawQuietZonesChanged(bool value)
    {
        _core.DrawQuietZones = value;
        _ = GenerateImageAsync();
    }

    partial void OnForegroundColorChanged(Color value)
    {
        _core.ForegroundColor = ColorConverter.ConvertToVectSharpColour(value);
        _ = GenerateImageAsync();
    }

    partial void OnModuleSizeChanged(int value)
    {
        _core.ModuleSize = value;
        _ = GenerateImageAsync();
    }

    partial void OnUseUtf8BomChanged(bool value)
    {
        _core.UseUtf8Bom = value;
        _core.GenerateQrCodeData();
        _ = GenerateImageAsync();
    }
}