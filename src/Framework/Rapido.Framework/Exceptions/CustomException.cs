namespace Rapido.Framework.Exceptions;

public abstract class CustomException : Exception
{
    public ExceptionCategory Category { get; }

    protected CustomException(string message, ExceptionCategory category) : base(message)
    {
        Category = category;
    }
}