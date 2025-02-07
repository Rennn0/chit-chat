#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace API.Source;

public class ResponseModelBase<T>
{
    public T Data { get; set; }
    public bool Success { get; set; } = true;
    public object? Error { get; set; } = null;
}
