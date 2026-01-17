using static QRCoder.PayloadGenerator.Geolocation;

namespace QrCodeGenerator.ViewModels;

public partial class GeolocationGeneratorPageViewModel : GeneratorViewModelBase
{
    private readonly GeolocationGenerator _generator = GeolocationGenerator.Instance;
    
    [ObservableProperty] private bool _isGeoSelected;
    [ObservableProperty] private bool _isGoogleMapsSelected;
    [ObservableProperty] private string _latitude;
    [ObservableProperty] private string _longitude;

    public GeolocationGeneratorPageViewModel()
    {
        _latitude = _generator.Latitude ?? string.Empty;
        _longitude = _generator.Longitude ?? string.Empty;

        if (_generator.GeolocationEncoding == GeolocationEncoding.GEO)
        {
            _isGeoSelected = true;
        }
        else
        {
            _isGoogleMapsSelected = true;
        }
    }

    partial void OnIsGeoSelectedChanged(bool value)
    {
        _generator.GeolocationEncoding = GeolocationEncoding.GEO;
        _ = GeneratePayload(_generator);
    }

    partial void OnIsGoogleMapsSelectedChanged(bool value)
    {
        _generator.GeolocationEncoding = GeolocationEncoding.GoogleMaps;
        _ = GeneratePayload(_generator);
    }

    partial void OnLatitudeChanged(string? oldValue, string newValue)
    {
        if (IsInputFieldEmpty(oldValue, newValue, "Latitude"))
        {
            return;
        }

        _generator.Latitude = newValue;
        _ = GeneratePayload(_generator);
    }

    partial void OnLongitudeChanged(string? oldValue, string newValue)
    {
        if (IsInputFieldEmpty(oldValue, newValue, "Longitude"))
        {
            return;
        }

        _generator.Longitude = newValue;
        _ = GeneratePayload(_generator);
    }
}