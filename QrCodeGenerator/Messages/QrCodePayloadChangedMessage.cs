namespace QrCodeGenerator.Messages;

public class QrCodePayloadChangedMessage(string? value) : ValueChangedMessage<string?>(value);
