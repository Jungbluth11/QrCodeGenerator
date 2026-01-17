namespace QrCodeGenerator.ViewModels;

public partial class TextGeneratorPageViewModel : GeneratorViewModelBase
{
    private readonly TextGenerator _generator = TextGenerator.Instance;

    [ObservableProperty] private string _text;
    [ObservableProperty] private bool _isRemoveWhitespaceChecked;

    public TextGeneratorPageViewModel()
    {
        _text = _generator.Text;
        _isRemoveWhitespaceChecked = _generator.RemoveWhitespace;
    }

    partial void OnTextChanged(string value)
    {
        _generator.Text = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnIsRemoveWhitespaceCheckedChanged(bool value)
    {
        _generator.RemoveWhitespace = value;
        _ = GeneratePayload(_generator);
    }
}
