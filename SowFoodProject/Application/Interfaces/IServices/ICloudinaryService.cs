using CloudinaryDotNet.Actions;

namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface ICloudinaryService
    {
        Task<DeletionResult> DeletePhotoAsync(string publicUrl);
        Task<Tuple<string, string>> UploadImage(IFormFile file, string userId);

    }
}