namespace Utilities;

public static class Exception<TException> where TException : Exception, new() {
    /// <summary>
    /// Creates and throws an instance of the exception whe predicate() returns true
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="message"></param>
    public static void ThrowOn(Func<bool> predicate, string? message = null)
    {
        if (!predicate()) return;
        
        if (message == null) {
            throw new TException();
        }
        
        throw Activator.CreateInstance(typeof(TException), message) as TException ?? new TException();
    }

    /// <summary>
    /// Creates and throws an instance of the exception whe predicate() returns true
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="messageFunc"></param>
    public static void ThrowOn(Func<bool> predicate, Func<string>? messageFunc = null)
    {
        if (!predicate()) return;
        
        var message = messageFunc?.Invoke();
        
        if (message == null) {
            throw new TException();
        }
        
        throw Activator.CreateInstance(typeof(TException), message) as TException ?? new TException();
    }
}