namespace QrCodeGenerator.ViewModels;

public partial class BookmarkGeneratorPageViewModel : GeneratorViewModelBase
{
    private readonly BookmarkGenerator _generator = BookmarkGenerator.Instance;

    [ObservableProperty] private string _url;
    [ObservableProperty] private string _title;
    [ObservableProperty] private bool _showTextOnImage;

    public BookmarkGeneratorPageViewModel()
    {
        _url = _generator.Url ?? string.Empty;
        _title = _generator.Title;
    }

    partial void OnUrlChanged(string? oldValue, string newValue)
    {
        if (IsInputFieldEmpty(oldValue, newValue, "URL"))
        {
            return;
        }

        _generator.Url = newValue;
        _ = GeneratePayload(_generator);
        UpdateImageText();
    }

    partial void OnTitleChanged(string? oldValue, string newValue)
    {
        _generator.Title = newValue;
        _ = GeneratePayload(_generator);
        UpdateImageText();
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