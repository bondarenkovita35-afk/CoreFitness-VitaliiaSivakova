namespace Domain.Exceptions;

// Kastas när medlemskap innehåller ogiltiga värden
public class InvalidMembershipException : DomainException
{
    public InvalidMembershipException(string message) : base(message)
    {
    }
}