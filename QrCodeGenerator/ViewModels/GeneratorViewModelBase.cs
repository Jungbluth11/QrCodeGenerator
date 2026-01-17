namespace QrCodeGenerator.ViewModels;

public abstract class GeneratorViewModelBase : ObservableObject
{
    protected async Task GeneratePayload<T>(T payloadGenerator) where T : IGenerator
    {
        try
        {
            string? payload = payloadGenerator.GeneratePayload();

            if (payload != null)
            {
                bool isPayloadLengthValid = await Core.Instance.IsPayloadLengthValidAsync(payload);
                
                if (!isPayloadLengthValid)
                {
                    return;
                }
            }


            WeakReferenceMessenger.Default.Send(new QrCodePayloadChangedMessage(payload));
        }
        catch (Exception ex)
        {
            Log.Error("Error Message: {message} - Stacktrace: {stacktrace}", ex.Message, ex.StackTrace);
            WeakReferenceMessenger.Default.Send(new ErrorMessage(ex.Message));
        }
    }

    protected bool IsInputFieldEmpty(string? oldValue, string newValue, string inputField)
    {
        if (!string.IsNullOrWhiteSpace(newValue) || string.IsNullOrWhiteSpace(oldValue))
        {
            return false;
        }

        WeakReferenceMessenger.Default.Send(new ErrorMessage(
            Core.StringLocalizer["StringErrorEmptyMessage"],
            inputField
        ));

        return true;
    }
}