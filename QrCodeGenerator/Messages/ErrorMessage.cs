namespace QrCodeGenerator.Messages;

public class ErrorMessage(string message, string? inputField = null)
{
    public string Message { get;} = message;
    public string? InputField { get; } = inputField;
}
