namespace Domain.Exceptions;

// Bas-exception för domänfel
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}