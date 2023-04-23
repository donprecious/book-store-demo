using BookStore.Application.Common.Models;

namespace BookStore.Application.Interfaces;

public interface IFileUpload
{
    Task<Result<FileUploadResponse>> UploadFile(string strFileName, Stream stream);
}

public record FileUploadResponse
{
    public string FileName { get; set; }
    public string Url { get; set; }
}