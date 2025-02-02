using Bal.Infrastracture;
using Microsoft.AspNetCore.Http;

namespace Bal.ServiceContracts;

public interface IFileFactory 
{
    IFile CreateFile(IFormFile file);
}