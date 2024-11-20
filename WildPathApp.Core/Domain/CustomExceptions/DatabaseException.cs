namespace WildPathApp.Core.Domain.CustomExceptions;

public class DatabaseException : Exception
{
    public DatabaseException(string message, Exception innerException) : base(message, innerException)
    {

    }
}