using Bal.Infrastracture;
using Bal.ServiceContracts;
using Microsoft.AspNetCore.Http;

namespace Bal.Helpers;

public class FileFactory :IFileFactory
{
    public IFile CreateFile(IFormFile formFile)
    {
        return new FormFileAdapter(formFile);
    }
}