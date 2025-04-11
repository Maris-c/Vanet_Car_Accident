namespace AccidentMonitor.Infrastructure.ORS;

[Serializable]
internal class UnknownException : Exception
{
    public UnknownException()
    {
    }

    public UnknownException(string? message) : base(message)
    {
    }

    public UnknownException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
