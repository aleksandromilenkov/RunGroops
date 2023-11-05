using CloudinaryDotNet.Actions;

namespace RunGroops.Interfaces {
    public interface IPhotoService {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);

    }
}
