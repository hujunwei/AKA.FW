namespace Utilities;

public class EntityUpdateException : Exception
{
    public EntityUpdateException()
        : base()
    { }

    public EntityUpdateException(string message)
        : base(message)
    { }

    public EntityUpdateException(string message, Exception innerException)
        : base(message, innerException)
    { }
}