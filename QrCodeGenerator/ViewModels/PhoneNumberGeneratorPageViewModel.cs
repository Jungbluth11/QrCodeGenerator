namespace QrCodeGenerator.ViewModels;

public partial class PhoneNumberGeneratorPageViewModel : GeneratorViewModelBase
{
    private readonly PhoneNumberGenerator _generator = PhoneNumberGenerator.Instance;

    [ObservableProperty] private bool _isUseGroupsVisible;
    [ObservableProperty] private bool _showTextOnImage;
    [ObservableProperty] private bool _useGroups;
    [ObservableProperty] private int _groupLength;
    [ObservableProperty] private string _phoneNumber;

    public PhoneNumberGeneratorPageViewModel()
    {
        _groupLength = _generator.GroupLength;
        _phoneNumber = _generator.PhoneNumber ?? string.Empty;
        _useGroups = _generator.UseGroups;
    }

    partial void OnGroupLengthChanged(int value)
    {
        _generator.GroupLength = value;
        UpdateImageText();
    }

    partial void OnPhoneNumberChanged(string? oldValue, string newValue)
    {
        if (IsInputFieldEmpty(oldValue, newValue, Core.StringLocalizer["PhoneNumber.Text"]))
        {
            return;
        }

        _generator.PhoneNumber = newValue;
        _ = GeneratePayload(_generator);
        UpdateImageText();
    }

    partial void OnShowTextOnImageChanged(bool value)
    {
        IsUseGroupsVisible = value;
        WeakReferenceMessenger.Default.Send(new ShowTextOnImageChangedMessage(value, _generator.GetImageText()));
    }

    partial void OnUseGroupsChanged(bool value)
    {
        _generator.UseGroups = value;
        UpdateImageText();
    }

    private void UpdateImageText()
    {
        if (ShowTextOnImage)
        {
            WeakReferenceMessenger.Default.Send(new ShowTextOnImageChangedMessage(_generator.GetImageText()));
        }
    }
}