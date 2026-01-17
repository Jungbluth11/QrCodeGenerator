using static QRCoder.PayloadGenerator.WiFi;

namespace QrCodeGenerator.Models;

public class WifiGenerator : IGenerator
{
    public Authentication AuthenticationMode { get; set; } = Authentication.WPA2;

    public static string[] AuthenticationModeStrings =>
    [
        Core.StringLocalizer["StringNone"],
        "WEP",
        "WPA",
        "WPA2"
    ];

    public int GroupLength { get; set; } = 4;
    public static WifiGenerator Instance => field ??= new();
    public bool IsHiddenNetwork { get; set; }
    public bool IsPasswordHex { get; set; }
    public string Password { get; set; } = string.Empty;
    public string? Ssid { get; set; } = null;
    public bool UseGroups { get; set; }

    private WifiGenerator()
    {
    }

    public string? GeneratePayload()
    {
        return Ssid == null 
            ? null 
            : new PayloadGenerator.WiFi(Ssid, Password, AuthenticationMode, IsHiddenNetwork, IsPasswordHex).ToString();
    }

    public string GetImageText()
    {
        if (Ssid == null)
        {
            return string.Empty;
        }

        string password = string.Empty;

        if (UseGroups && AuthenticationMode != Authentication.nopass)
        {
            password = Core.FormatText(Password, GroupLength);
        }
        else if (AuthenticationMode != Authentication.nopass)
        {
            password = Password;
        }

        string ssidText = $"SSID: {Ssid}";
        string passwordText = $"{Core.StringLocalizer["Password.Text"]}: {password}";

        return password == string.Empty ? ssidText : $"{ssidText}\n{passwordText}";
    }
}
