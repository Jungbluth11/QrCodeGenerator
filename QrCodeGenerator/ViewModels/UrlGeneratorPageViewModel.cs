namespace QrCodeGenerator.ViewModels;

public partial class UrlGeneratorPageViewModel : GeneratorViewModelBase
{
    private readonly UrlGenerator _generator = UrlGenerator.Instance;

    [ObservableProperty] private string _url;

    public UrlGeneratorPageViewModel()
    {
        _url = _generator.Url ?? string.Empty;
    }

    partial void OnUrlChanged(string? oldValue, string newValue)
    {
        if (IsInputFieldEmpty(oldValue, newValue, "URL"))
        {
            return;
        }

        _generator.Url = newValue;
        _ = GeneratePayload(_generator);
    }
}