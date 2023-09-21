using Amazon.S3;
using Amazon.S3.Model;
using Reviewing.Application.Services;
using System.Net;

namespace Reviewing.Infrastructure.S3;

public class ImageService
    : IImageService
{
    private readonly IAmazonS3 _s3;
    private const string _bucketName = "itransition-reviews";
    private const string _previewImageFormat = "preview_images/{0}";

    public ImageService(IAmazonS3 s3)
    {
        _s3 = s3;
    }

    public async Task<string?> UploadImageAsync(string id, string? contentType, Stream? inputStream)
    {
        if (string.IsNullOrEmpty(contentType) || inputStream == null) return null;

        string key = string.Format(_previewImageFormat, id);
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            ContentType = contentType,
            InputStream = inputStream,
            CannedACL = S3CannedACL.PublicRead
        };

        var response = await _s3.PutObjectAsync(request);
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            var bucketRegion = await _s3.GetBucketLocationAsync(_bucketName);
            string url = $"https://{_bucketName}.s3.{bucketRegion.Location}.amazonaws.com/{key}";
            return url;
        }

        return null;
    }

    public async Task<bool> CheckImageExistAsync(string id, ImageS3Folder imageS3Folder)
    {
        return (await _s3.GetObjectAsync(_bucketName, imageS3Folder switch
        {
            ImageS3Folder.PreviewImages => string.Format(_previewImageFormat, id),
            _ => throw new ArgumentException(null, nameof(imageS3Folder))
        })) is not null;
    }

    public async Task<(Stream, string)?> GetImageAsync(string id, ImageS3Folder imageS3Folder)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = imageS3Folder switch
                {
                    ImageS3Folder.PreviewImages => string.Format(_previewImageFormat, id),
                    _ => throw new ArgumentException(null, nameof(imageS3Folder))
                }
            };

            var response = await _s3.GetObjectAsync(request);
            return (response.ResponseStream, response.Headers.ContentType);
        }
        catch (AmazonS3Exception ex) when (ex.Message is "The specified key does not exist.")
        {
            return null;
        }
    }
}