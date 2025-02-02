using Bal.Infrastracture;

namespace Bal.ServiceContracts;

public interface IFileService
{
    FileResult UploadImage(IFile file);
}