using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using MyDigitalCv.Helpers;
using MyDigitalCv.Interfaces;

namespace MyDigitalCv.Services
{
	public class PhotoService : IPhotoService
	{
		private readonly Cloudinary _cloudinary;
		public PhotoService(IOptions<CloudinarySettings> config)
		{
			var account = new Account(
				config.Value.CloudName,
				config.Value.ApiKey,
				config.Value.ApiSecret);
			_cloudinary = new Cloudinary(account);
		}

		public async Task<ImageUploadResult> AddPhotoAsync(IFormFile image)
		{
			var imageUploadResult = new ImageUploadResult();
			if (image.Length > 0)
			{
				using var stream = image.OpenReadStream();
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(image.FileName, stream),
					Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
				};
				imageUploadResult = await _cloudinary.UploadAsync(uploadParams);
			}			
			return imageUploadResult;
		}

		

		public async Task<DeletionResult> DeletePhotoAsync(string publicId)
		{
			var deleteParams = new DeletionParams(publicId);
			var result = await _cloudinary.DestroyAsync(deleteParams);
			return result;
		}
	}
}
