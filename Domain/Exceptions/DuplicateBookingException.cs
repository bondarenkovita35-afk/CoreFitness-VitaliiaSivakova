namespace Domain.Exceptions;

// Kastas när samma pass bokas två gånger
public class DuplicateBookingException : DomainException
{
    public DuplicateBookingException()
        : base("Du har redan bokat detta pass.")
    {
    }
}