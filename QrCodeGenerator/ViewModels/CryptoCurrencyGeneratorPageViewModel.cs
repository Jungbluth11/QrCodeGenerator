#pragma warning disable CS8618 
using static QRCoder.PayloadGenerator.BitcoinLikeCryptoCurrencyAddress;

namespace QrCodeGenerator.ViewModels;

public partial class CryptoCurrencyGeneratorPageViewModel : GeneratorViewModelBase
{
    private readonly CryptoCurrencyGenerator _generator = CryptoCurrencyGenerator.Instance;

    [ObservableProperty] private bool _isBitcoinCashSelected;
    [ObservableProperty] private bool _isBitcoinSelected;
    [ObservableProperty] private bool _isLitecoinSelected;
    [ObservableProperty] private bool _showTextOnImage;
    [ObservableProperty] private double _amount;
    [ObservableProperty] private string _address;
    [ObservableProperty] private string _addressDescriptor;
    [ObservableProperty] private string _showTextOnImageDescriptor;

    public CryptoCurrencyGeneratorPageViewModel()
    {
        _amount = _generator.Amount ?? double.NaN;
        _address = _generator.Address ?? string.Empty;

        switch (_generator.CurrencyType)
        {
            case BitcoinLikeCryptoCurrencyType.Bitcoin:
                _isBitcoinSelected = true;
                break;
            case BitcoinLikeCryptoCurrencyType.BitcoinCash:
                _isBitcoinCashSelected = true;
                break;
            case BitcoinLikeCryptoCurrencyType.Litecoin:
                _isLitecoinSelected = true;
                break;
        }

        SetCurrencyType();
    }

    private void SetCurrencyType()
    {
        AddressDescriptor = $"{_generator.GetCurrencyDescriptior()}*";
        ShowTextOnImageDescriptor = ((string)Core.StringLocalizer["ShowCryptoCurrencyAddressOnQrCode"])
            .Replace("{CryptoCurrency}", _generator.GetCurrencyDescriptior());

        if (string.IsNullOrWhiteSpace(Address))
        {
            return;
        }
        
        _ = GeneratePayload(_generator);
        UpdateImageText();
    }

    partial void OnAddressChanged(string? oldValue, string newValue)
    {
        if (IsInputFieldEmpty(oldValue, newValue, _generator.GetCurrencyDescriptior()))
        {
            return;
        }

        _generator.Address = newValue;
        _ = GeneratePayload(_generator);
        UpdateImageText();
    }

    partial void OnAmountChanged(double value)
    {
        _generator.Amount = value is 0 or double.NaN ? null : value;
        _ = GeneratePayload(_generator);
    }

    partial void OnIsBitcoinCashSelectedChanged(bool value)
    {
        if (!value)
        {
            return;
        }

        IsBitcoinSelected = false;
        IsLitecoinSelected = false;
        _generator.CurrencyType = BitcoinLikeCryptoCurrencyType.BitcoinCash;
        SetCurrencyType();
    }

    partial void OnIsBitcoinSelectedChanged(bool value)
    {
        if (!value)
        {
            return;
        }

        IsBitcoinCashSelected = false;
        IsLitecoinSelected = false;
        _generator.CurrencyType = BitcoinLikeCryptoCurrencyType.Bitcoin;
        SetCurrencyType();
    }

    partial void OnIsLitecoinSelectedChanged(bool value)
    {
        if (!value)
        {
            return;
        }

        IsBitcoinSelected = false;
        IsBitcoinCashSelected = false;
        _generator.CurrencyType = BitcoinLikeCryptoCurrencyType.Litecoin;
        SetCurrencyType();
    }

    partial void OnShowTextOnImageChanged(bool value)
    {
        WeakReferenceMessenger.Default.Send(new ShowTextOnImageChangedMessage(value, _generator.GetImageText()));
    }

    private void UpdateImageText()
    {
        if (ShowTextOnImage)
        {
            WeakReferenceMessenger.Default.Send(new ShowTextOnImageChangedMessage(_generator.GetImageText()));
        }
    }
}