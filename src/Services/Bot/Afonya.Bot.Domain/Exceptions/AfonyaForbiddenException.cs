namespace Afonya.Bot.Domain.Exceptions;

[Serializable]

public class AfonyaForbiddenException : AfonyaErrorException
{
    public AfonyaForbiddenException(string message) : base(message)
    {
    }
}