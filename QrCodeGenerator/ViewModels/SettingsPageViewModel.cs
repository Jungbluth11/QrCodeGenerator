namespace QrCodeGenerator.ViewModels;

public partial class SettingsPageViewModel : ObservableObject
{
    private readonly Core _core = Core.Instance;

    [ObservableProperty] private bool _isTextGeneratorEnabled;
    [ObservableProperty] private bool _isWiFiGeneratorEnabled;
    [ObservableProperty] private bool _isUrlGeneratorEnabled;
    [ObservableProperty] private bool _isBookmarkGeneratorEnabled;
    [ObservableProperty] private bool _isMailGeneratorEnabled;
    [ObservableProperty] private bool _isSmsGeneratorEnabled;
    [ObservableProperty] private bool _isMmsGeneratorEnabled;
    [ObservableProperty] private bool _isGeoGeneratorlocationEnabled;
    [ObservableProperty] private bool _isPhoneNumberGeneratorEnabled;
    [ObservableProperty] private bool _isWhatsAppMessageGeneratorEnabled;
    [ObservableProperty] private bool _isContactDataGeneratorEnabled;
    [ObservableProperty] private bool _isCryptoCurrencyEnabled;
    [ObservableProperty] private int _currentTheme;


    public SettingsPageViewModel()
    {
        CurrentTheme = _core.Theme switch
        {
            "Light" => 1,
            "Dark" => 2,
            _ => 0
        };

        IsTextGeneratorEnabled = _core.LoadedGenerators.Contains("Text");
        IsWiFiGeneratorEnabled = _core.LoadedGenerators.Contains("WiFi");
        IsUrlGeneratorEnabled = _core.LoadedGenerators.Contains("URL");
        IsBookmarkGeneratorEnabled = _core.LoadedGenerators.Contains("Bookmark");
        IsMailGeneratorEnabled = _core.LoadedGenerators.Contains("Mail");
        IsSmsGeneratorEnabled = _core.LoadedGenerators.Contains("SMS");
        IsMmsGeneratorEnabled = _core.LoadedGenerators.Contains("MMS");
        IsGeoGeneratorlocationEnabled = _core.LoadedGenerators.Contains("Geolocation");
        IsPhoneNumberGeneratorEnabled = _core.LoadedGenerators.Contains("PhoneNumber");
        IsWhatsAppMessageGeneratorEnabled = _core.LoadedGenerators.Contains("WhatsAppMessage");
        IsContactDataGeneratorEnabled = _core.LoadedGenerators.Contains("ContactData");
        IsCryptoCurrencyEnabled = _core.LoadedGenerators.Contains("CryptoCurrency");

        PropertyChanged += SettingsPageViewModel_PropertyChanged;
    }

    private void SettingsPageViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "CurrentTheme")
        {
            return;
        }

        string generatorName = e.PropertyName! switch
        {

            "IsWiFiGeneratorEnabled" => "WiFi",
            "IsUrlGeneratorEnabled" => "URL",
            "IsBookmarkGeneratorEnabled" => "Bookmark",
            "IsMailGeneratorEnabled" => "Mail",
            "IsSmsGeneratorEnabled" => "SMS",
            "IsMmsGeneratorEnabled" => "MMS",
            "IsGeoGeneratorlocationEnabled" => "Geolocation",
            "IsPhoneNumberGeneratorEnabled" => "PhoneNumber",
            "IsWhatsAppMessageGeneratorEnabled" => "WhatsAppMessage",
            "IsContactDataGeneratorEnabled" => "ContactData",
            "IsCryptoCurrencyEnabled" => "CryptoCurrency",
            _ => "Text"
        };

        _core.ToggleGenerator(generatorName);
        WeakReferenceMessenger.Default.Send(new LoadedGeneratorsChangedMessage(generatorName));
    }
}
