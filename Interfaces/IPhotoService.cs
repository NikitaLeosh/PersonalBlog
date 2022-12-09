using CloudinaryDotNet.Actions;

namespace MyDigitalCv.Interfaces
{
	public interface IPhotoService
	{
		Task<ImageUploadResult> AddPhotoAsync(IFormFile image);
		Task<DeletionResult> DeletePhotoAsync(string publicId);
	}
}
