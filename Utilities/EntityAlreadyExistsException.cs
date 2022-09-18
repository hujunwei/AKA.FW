namespace Utilities;

public class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException()
        : base()
    { }

    public EntityAlreadyExistsException(string message)
        : base(message)
    { }

    public EntityAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
