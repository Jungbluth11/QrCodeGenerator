using static QRCoder.PayloadGenerator.ContactData;

namespace QrCodeGenerator.Models;

public class ContactDataGenerator : IGenerator
{
    public static ContactDataGenerator Instance => field ??= new();

    public static string[] OutputTypeStrings =>
    [
        "VCard v2.1",
        "VCard v3",
        "VCard v4",
        "MeCard"
    ];

    public AddressOrder AddressOrder { get; set; } = AddressOrder.Default;

    public DateTime? Birthday { get; set; }

    public string City { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Firstname { get; set; } = string.Empty;

    public string HouseNumber { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public string MobilePhone { get; set; } = string.Empty;

    public string Nickname { get; set; } = string.Empty;

    public string Note { get; set; } = string.Empty;

    public string Org { get; set; } = string.Empty;

    public string OrgTitle { get; set; } = string.Empty;

    public ContactOutputType OutputType { get; set; } = ContactOutputType.VCard4;

    public string Phone { get; set; } = string.Empty;

    public string StateRegion { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string Website { get; set; } = string.Empty;

    public string WorkPhone { get; set; } = string.Empty;

    public string ZipCode { get; set; } = string.Empty;

    private ContactDataGenerator()
    {
    }

    public string? GeneratePayload()
    {
        if (string.IsNullOrWhiteSpace(Firstname) && string.IsNullOrWhiteSpace(Lastname) && string.IsNullOrWhiteSpace(Nickname))
        {
            throw new(Core.StringLocalizer["StringErrorContactDataName"]);
        }

        return new PayloadGenerator.ContactData(OutputType,
                Firstname,
                Lastname,
                Nickname,
                Phone,
                MobilePhone,
                WorkPhone,
                Email,
                Birthday,
                Website,
                Street,
                HouseNumber,
                City,
                ZipCode,
                Country,
                Note,
                StateRegion,
                AddressOrder,
                Org,
                OrgTitle)
            .ToString();
    }
}