namespace Bal.Infrastracture;

public interface IFile
{
    public string  FileName { get; }
    public Stream Content { get;  }
    public int Length { get;  }
}