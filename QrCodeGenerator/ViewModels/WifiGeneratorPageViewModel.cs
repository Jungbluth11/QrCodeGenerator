using static QRCoder.PayloadGenerator.WiFi;

namespace QrCodeGenerator.ViewModels;

public partial class WifiGeneratorPageViewModel : GeneratorViewModelBase
{
    private readonly WifiGenerator _generator = WifiGenerator.Instance;
    [ObservableProperty] private bool _isAsciiSelected;
    [ObservableProperty] private bool _isHexSelected;
    [ObservableProperty] private bool _isHiddenNetwork;
    [ObservableProperty] private bool _isPasswordBoxEnabled;
    [ObservableProperty] private bool _isUseGroupsVisible;
    [ObservableProperty] private bool _showTextOnImage;
    [ObservableProperty] private bool _useGroups;
    [ObservableProperty] private int _groupLength;
    [ObservableProperty] private string _currentAuthenticationMode;
    [ObservableProperty] private string _password;
    [ObservableProperty] private string _ssid;

    public string[] AuthenticationModeStrings => WifiGenerator.AuthenticationModeStrings;

    public WifiGeneratorPageViewModel()
    {
        _ssid = _generator.Ssid ?? string.Empty;
        _password = _generator.Password;

        _currentAuthenticationMode = _generator.AuthenticationMode switch
        {
            Authentication.WEP => "WEP",
            Authentication.WPA => "WPA",
            Authentication.WPA2 => "WPA2",
            _ => Core.StringLocalizer["StringNone"]
        };

        _isHiddenNetwork = _generator.IsHiddenNetwork;

        if (_generator.IsPasswordHex)
        {
            _isHexSelected = true;
            _isAsciiSelected = false;
        }
        else
        {
            _isHexSelected = false;
            _isAsciiSelected = true;
        }

        _groupLength = _generator.GroupLength;
        _useGroups = _generator.UseGroups;
    }

    private void GeneratePayload()
    {
        _ = GeneratePayload(_generator);
        UpdateImageText();
    }

    partial void OnCurrentAuthenticationModeChanged(string value)
    {
        _generator.AuthenticationMode = value switch
        {
            "WEP" => Authentication.WEP,
            "WPA" => Authentication.WPA,
            "WPA2" => Authentication.WPA2,
            _ => Authentication.nopass
        };

        IsPasswordBoxEnabled = _generator.AuthenticationMode == Authentication.nopass;
        GeneratePayload();
    }

    partial void OnGroupLengthChanged(int value)
    {
        _generator.GroupLength = value;
        UpdateImageText();
    }

    partial void OnIsHexSelectedChanged(bool value)
    {
        _generator.IsPasswordHex = value;
        IsUseGroupsVisible = !value;
        GeneratePayload();
    }

    partial void OnIsHiddenNetworkChanged(bool value)
    {
        _generator.IsHiddenNetwork = value;
        GeneratePayload();
    }

    partial void OnPasswordChanged(string? oldValue, string newValue)
    {
        if (_generator.AuthenticationMode != Authentication.nopass &&
            IsInputFieldEmpty(oldValue, newValue, Core.StringLocalizer["Password.Text"]))
        {
            return;
        }
        
        _generator.Password = newValue;
        GeneratePayload();
    }

    partial void OnShowTextOnImageChanged(bool value)
    {
        if (value && IsAsciiSelected)
        {
            IsUseGroupsVisible = true;
        }
        else
        {
            IsUseGroupsVisible = false;
        }

        WeakReferenceMessenger.Default.Send(new ShowTextOnImageChangedMessage(value, _generator.GetImageText()));
    }

    partial void OnSsidChanged(string? oldValue, string newValue)
    {
        if (IsInputFieldEmpty(oldValue, newValue, "SSID"))
        {
            return;
        }

        _generator.Ssid = newValue;
        GeneratePayload();
    }

    partial void OnUseGroupsChanged(bool value)
    {
        _generator.UseGroups = value;
        UpdateImageText();
    }

    private void UpdateImageText()
    {
        if (ShowTextOnImage)
        {
            WeakReferenceMessenger.Default.Send(new ShowTextOnImageChangedMessage(_generator.GetImageText()));
        }
    }
}
