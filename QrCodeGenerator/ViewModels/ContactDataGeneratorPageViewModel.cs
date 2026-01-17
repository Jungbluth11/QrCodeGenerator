using static QRCoder.PayloadGenerator.ContactData;

namespace QrCodeGenerator.ViewModels;

public partial class ContactDataGeneratorPageViewModel : GeneratorViewModelBase
{
    private readonly ContactDataGenerator _generator = ContactDataGenerator.Instance;
    
    [ObservableProperty] private bool _isAdressFormatEuropeSelected;
    [ObservableProperty] private bool _isAdressFormatNorthAmericaAndOtherSelected;
    [ObservableProperty] private DateTimeOffset? _birthday;
    [ObservableProperty] private string _city;
    [ObservableProperty] private string _country;
    [ObservableProperty] private string _email;
    [ObservableProperty] private string _firstname;
    [ObservableProperty] private string _houseNumber;
    [ObservableProperty] private string _lastname;
    [ObservableProperty] private string _mobilePhone;
    [ObservableProperty] private string _nickname;
    [ObservableProperty] private string _note;
    [ObservableProperty] private string _org;
    [ObservableProperty] private string _orgTitle;
    [ObservableProperty] private string _phone;
    [ObservableProperty] private string _selectedOutputType;
    [ObservableProperty] private string _stateRegion;
    [ObservableProperty] private string _street;
    [ObservableProperty] private string _website;
    [ObservableProperty] private string _workPhone;
    [ObservableProperty] private string _zipCode;

    public string[] OutputTypes => ContactDataGenerator.OutputTypeStrings;

    public ContactDataGeneratorPageViewModel()
    {
        _firstname = _generator.Firstname;
        _lastname = _generator.Lastname;
        _nickname = _generator.Nickname;
        _phone = _generator.Phone;
        _mobilePhone = _generator.MobilePhone;
        _workPhone = _generator.WorkPhone;
        _email = _generator.Email;
        _website = _generator.Website;
        _street = _generator.Street;
        _houseNumber = _generator.HouseNumber;
        _city = _generator.City;
        _zipCode = _generator.ZipCode;
        _country = _generator.Country;
        _note = _generator.Note;
        _stateRegion = _generator.StateRegion;
        _org = _generator.Org;
        _orgTitle = _generator.OrgTitle;
        _selectedOutputType = OutputTypes[0];
        _birthday = _generator.Birthday;

        if (_generator.AddressOrder == AddressOrder.Default)
        {
            _isAdressFormatEuropeSelected = true;
        }
        else
        {
            _isAdressFormatNorthAmericaAndOtherSelected = true;
        }
    }

    partial void OnBirthdayChanged(DateTimeOffset? value)
    {
        _generator.Birthday = value?.DateTime;
    }

    partial void OnCityChanged(string value)
    {
        _generator.City = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnCountryChanged(string value)
    {
        _generator.Country = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnEmailChanged(string value)
    {
        _generator.Email = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnFirstnameChanged(string value)
    {
        _generator.Firstname = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnHouseNumberChanged(string value)
    {
        _generator.HouseNumber = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnIsAdressFormatEuropeSelectedChanged(bool value)
    {
        _generator.AddressOrder = AddressOrder.Default;
    }

    partial void OnIsAdressFormatNorthAmericaAndOtherSelectedChanged(bool value)
    {
        _generator.AddressOrder = AddressOrder.Reversed;
    }

    partial void OnLastnameChanged(string value)
    {
        _generator.Lastname = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnMobilePhoneChanged(string value)
    {
        _generator.MobilePhone = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnNicknameChanged(string value)
    {
        _generator.Nickname = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnNoteChanged(string value)
    {
        _generator.Note = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnOrgChanged(string value)
    {
        _generator.Org = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnOrgTitleChanged(string value)
    {
        _generator.OrgTitle = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnPhoneChanged(string value)
    {
        _generator.Phone = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnSelectedOutputTypeChanged(string value)
    {
        _generator.OutputType = value switch
        {
            "VCard v2.1" => ContactOutputType.VCard21,
            "VCard v3" => ContactOutputType.VCard3,
            "VCard v4" => ContactOutputType.VCard4,
            _ => ContactOutputType.MeCard
        };
    }

    partial void OnStateRegionChanged(string value)
    {
        _generator.StateRegion = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnStreetChanged(string value)
    {
        _generator.Street = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnWebsiteChanged(string value)
    {
        _generator.Website = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnWorkPhoneChanged(string value)
    {
        _generator.WorkPhone = value;
        _ = GeneratePayload(_generator);
    }

    partial void OnZipCodeChanged(string value)
    {
        _generator.ZipCode = value;
        _ = GeneratePayload(_generator);
    }
}