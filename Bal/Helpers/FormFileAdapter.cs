using Bal.Infrastracture;
using Microsoft.AspNetCore.Http;

namespace Bal.Helpers;

public class FormFileAdapter : IFile
{
    private readonly IFormFile _formFile;
    public FormFileAdapter(IFormFile formFile)
    {
        _formFile = formFile ?? throw new ArgumentNullException(nameof(formFile));
        FileName = _formFile.FileName; 
        Content = _formFile.OpenReadStream();  
        Length = (int)_formFile.Length;  
    }
    public string FileName { get;  }  
    public Stream Content { get;}  
    public int Length { get; } 
}