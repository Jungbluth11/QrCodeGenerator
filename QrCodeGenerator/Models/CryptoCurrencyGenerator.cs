using static QRCoder.PayloadGenerator.BitcoinLikeCryptoCurrencyAddress;

namespace QrCodeGenerator.Models;

public class CryptoCurrencyGenerator : IGenerator
{
    public string? Address { get; set; } = null;

    public double? Amount { get; set; } = null;

    public BitcoinLikeCryptoCurrencyType CurrencyType { get; set; } = BitcoinLikeCryptoCurrencyType.Bitcoin;
    public static CryptoCurrencyGenerator Instance => field ??= new();

    private CryptoCurrencyGenerator()
    {
    }

    public string? GeneratePayload()
    {
        return Address == null
            ? null
            : new PayloadGenerator.BitcoinLikeCryptoCurrencyAddress(CurrencyType, Address, Amount).ToString();
    }

    public string GetCurrencyDescriptior()
    {
        return CurrencyType switch
        {
            BitcoinLikeCryptoCurrencyType.Bitcoin => Core.StringLocalizer["StringBitcoinAddress"],
            BitcoinLikeCryptoCurrencyType.BitcoinCash => Core.StringLocalizer["StringBitcoinCashAddress"],
            _ => Core.StringLocalizer["StringLitecoinAddress"]
        };
    }

    public string GetImageText() => $"{GetCurrencyDescriptior()}: {Address}";
}