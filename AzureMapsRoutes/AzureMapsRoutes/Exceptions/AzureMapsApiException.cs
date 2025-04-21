namespace AzureMapsRoutes.Exceptions;

public class AzureMapsApiException : Exception
{
    public AzureMapsApiException(string? message) : base(message)
    {
    }
}