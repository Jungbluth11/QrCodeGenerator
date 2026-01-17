namespace QrCodeGenerator.ViewModels;

public partial class MmsGeneratorPageViewModel : GeneratorViewModelBase
{
    private readonly MmsGenerator _generator = MmsGenerator.Instance;

    [ObservableProperty] private string _phoneNumber;
    [ObservableProperty] private string _message;

    public MmsGeneratorPageViewModel()
    {
        _phoneNumber = _generator.PhoneNumber ?? string.Empty;
        _message = _generator.Message;
    }

    partial void OnPhoneNumberChanged(string? oldValue, string newValue)
    {
        if (IsInputFieldEmpty(oldValue, newValue, Core.StringLocalizer["PhoneNumber.Text"]))
        {
            return;
        }

        _generator.PhoneNumber = newValue;
        _ = GeneratePayload(_generator);
    }

    partial void OnMessageChanged(string value)
    {
        _generator.Message = value;
        _ = GeneratePayload(_generator);
    }
}