namespace Bal.Infrastracture;

public class FileResult
{
    public bool IsSuccess { get; set; } = true;
    public string FileName { get; set; }
    public string ErrorMessage { get; set; }
}