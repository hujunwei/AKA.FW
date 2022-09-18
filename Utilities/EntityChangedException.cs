namespace Utilities;

public class EntityChangedException : Exception
{
    public EntityChangedException()
        : base()
    { }

    public EntityChangedException(string message)
        : base(message)
    { }

    public EntityChangedException(string message, Exception innerException)
        : base(message, innerException)
    { }
}