
using Bal.Infrastracture;
using Bal.ServiceContracts;

namespace Bal.Services;

public class FileService : IFileService
{
    public FileResult UploadImage(IFile file)
    {
        List<string> validExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        string extension = Path.GetExtension(file.FileName);
        if (!validExtensions.Contains(extension))
        {
            return new FileResult
            {
                IsSuccess = false,
                ErrorMessage =
                    $"Invalid file extension {extension} (Allowed extensions: {string.Join(',', validExtensions)})"
            };
        }

        long size = file.Length;
        if (size > 5 * 1024 * 1024) 
        {
            return new FileResult { IsSuccess = false, ErrorMessage = "Maximum file size is 5MB" };
        }

        string fileName = Guid.NewGuid() + extension;
        string uploadsFolder = Path.Combine("Uploads");
        Directory.CreateDirectory(uploadsFolder);
        string filePath = Path.Combine(uploadsFolder, fileName);
        using var fileStream = new FileStream(filePath, FileMode.Create);
        file.Content.CopyTo(fileStream);


        return new FileResult { FileName = filePath };
    }
}