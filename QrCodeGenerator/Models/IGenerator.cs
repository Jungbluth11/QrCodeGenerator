namespace QrCodeGenerator.Models;

public interface IGenerator
{
    public string? GeneratePayload();
}
