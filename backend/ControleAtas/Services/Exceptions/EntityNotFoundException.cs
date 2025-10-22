namespace ControleAtas.Services.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) : base(message)
    {
    }

    public EntityNotFoundException(string message, object? details) : base(message)
    {
        Details = details;
    }

    public object? Details { get; }
}
