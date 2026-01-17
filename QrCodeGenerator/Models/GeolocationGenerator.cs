using static QRCoder.PayloadGenerator.Geolocation;

namespace QrCodeGenerator.Models;

public class GeolocationGenerator : IGenerator
{
    public static GeolocationGenerator Instance => field ??= new();

    public string? Latitude { get; set; } = null;
    
    public string? Longitude { get; set; } = null;

    public GeolocationEncoding GeolocationEncoding { get; set; } = GeolocationEncoding.GEO;

    private GeolocationGenerator()
    {
    }

    public string? GeneratePayload()
    {
        if (Latitude == null || Longitude == null)
        {
            return null;
        }

        return new PayloadGenerator.Geolocation(Latitude, Longitude, GeolocationEncoding).ToString();
    }
}