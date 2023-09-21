namespace Reviewing.Application.Services;

public enum ImageS3Folder
{
    PreviewImages = 0
}

public interface IImageService
{
    Task<string?> UploadImageAsync(string id, string? contentType, Stream? inputStream);
    Task<bool> CheckImageExistAsync(string id, ImageS3Folder imageS3Folder);
    Task<(Stream, string)?> GetImageAsync(string id, ImageS3Folder imageS3Folder);
}