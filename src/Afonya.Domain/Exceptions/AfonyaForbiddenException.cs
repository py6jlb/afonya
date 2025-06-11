namespace Afonya.Domain.Exceptions;

[Serializable]

public class AfonyaForbiddenException : AfonyaErrorException
{
    public AfonyaForbiddenException(string message) : base(message)
    {
    }
}