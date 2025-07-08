using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using SowFoodProject.Application.Interfaces.IServices;

namespace SowFoodProject.Infrastructure.Implementations.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary cloudinary;

    public CloudinaryService(IConfiguration config)
    {
        var cloudName = config.GetSection("Cloudinary:CloudName").Value;
        var apiKey = config.GetSection("Cloudinary:ApiKey").Value;
        var apiSecret = config.GetSection("Cloudinary:ApiSecret").Value;

        Account account = new Account
        {
            ApiKey = apiKey,
            ApiSecret = apiSecret,
            Cloud = cloudName
        };

        cloudinary = new Cloudinary(account);
    }

    public async Task<Tuple<string, string>> UploadImage(IFormFile file, string userId)
    {
        var response = new Dictionary<string, string>();
        var defaultSize = 800000; // in bytes (~0.8 MB)
        var allowedTypes = new List<string> { "image/jpeg", "image/jpg", "image/png", "application/pdf" };

        if (file == null)
        {
            return Tuple
                .Create("No file uploaded", "40");
        }

        if (file.Length < 1 || file.Length > defaultSize)
        {
            return Tuple
                .Create("Invalid file size", "40");
        }

        if (!allowedTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            return Tuple
                .Create("Invalid file type", "40");
        }

        // Upload result object
        UploadResult uploadResult;

        using var stream = file.OpenReadStream();

        if (file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
        {
            // Upload PDF using RawUploadParams
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = $"ASS/{userId}",
                UseFilename = true,
                UniqueFilename = true
            };
            uploadResult = await cloudinary.UploadAsync(uploadParams);
        }
        else
        {
            // Upload image using ImageUploadParams
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = $"ASS/{userId}"
            };
            uploadResult = await cloudinary.UploadAsync(uploadParams);
        }

        if (!string.IsNullOrEmpty(uploadResult.PublicId))
        {
            response.Add("PublicId", uploadResult.PublicId);
            response.Add("Url", uploadResult.SecureUrl?.ToString() ?? uploadResult.Url?.ToString());

            return Tuple
                 .Create(uploadResult.SecureUrl?.ToString() ?? uploadResult.Url?.ToString(), "20");

        };
        return Tuple.Create("Failed to upload", "50");

    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicUrl)
    {
        var publicId = publicUrl.Split('/').Last().Split('.')[0];
        var deleteParams = new DeletionParams(publicId);
        return await cloudinary.DestroyAsync(deleteParams);
    }

}
