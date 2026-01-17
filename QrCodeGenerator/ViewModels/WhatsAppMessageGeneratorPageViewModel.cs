namespace QrCodeGenerator.ViewModels;

public partial class WhatsAppMessageGeneratorPageViewModel : GeneratorViewModelBase
{
    private readonly WhatsAppMessageGenerator _generator = WhatsAppMessageGenerator.Instance;

    [ObservableProperty] private string _phoneNumber;
    [ObservableProperty] private string _message;

    public WhatsAppMessageGeneratorPageViewModel()
    {
        _phoneNumber = _generator.PhoneNumber;
        _message = _generator.Message ?? string.Empty;
    }

    partial void OnPhoneNumberChanged(string value)
    {
        _generator.PhoneNumber = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnMessageChanged(string? oldValue, string newValue)
    {
        if (IsInputFieldEmpty(oldValue, newValue, Core.StringLocalizer["Message.Text"]))
        {
            return;
        }

        _generator.Message = newValue;
        _ = GeneratePayload(_generator);
    }
}