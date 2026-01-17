namespace QrCodeGenerator.Messages;

public class LoadedGeneratorsChangedMessage(string value) : ValueChangedMessage<string>(value);
