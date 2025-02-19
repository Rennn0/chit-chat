namespace API.Source;

public class ResponseModelBase<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public object? Error { get; set; } = null;
}
