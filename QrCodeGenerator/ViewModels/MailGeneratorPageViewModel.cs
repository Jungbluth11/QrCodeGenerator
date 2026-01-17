namespace QrCodeGenerator.ViewModels;

public partial class MailGeneratorPageViewModel : GeneratorViewModelBase
{
    private readonly MailGenerator _generator = MailGenerator.Instance;

    [ObservableProperty] private string _receiver;
    [ObservableProperty] private string _subject;
    [ObservableProperty] private string _message;

    public MailGeneratorPageViewModel()
    {
        _receiver = _generator.Receiver;
        _subject = _generator.Subject;
        _message = _generator.Message;
    }

    partial void OnReceiverChanged(string value)
    {
        _generator.Receiver = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnSubjectChanged(string value)
    {
        _generator.Subject = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnMessageChanged(string value)
    {
        _generator.Message = value;
        _ = GeneratePayload(_generator);
    }
}