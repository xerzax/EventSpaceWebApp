//using Application.Interfaces.Services;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;



//namespace Infrastructure.Implementation.Services
//{
//	public class FileService : IFileService
//	{
//		private readonly IWebHostBuilder environment;
//		public FileService(IWebHostBuilder env)
//		{
//			this.environment = env;
//		}

//		public Tuple<int, string> SaveImage(IFormFile imageFile)
//		{
//			try
//			{
//				var wwwPath = this.environment.WebRootPath;
//				var path = Path.Combine(wwwPath, "Uploads");
//				if (!Directory.Exists(path))
//				{
//					Directory.CreateDirectory(path);
//				}

//				// Check the allowed extenstions
//				var ext = Path.GetExtension(imageFile.FileName);
//				var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
//				if (!allowedExtensions.Contains(ext))
//				{
//					string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
//					return new Tuple<int, string>(0, msg);
//				}
//				string uniqueString = Guid.NewGuid().ToString();
//				// we are trying to create a unique filename here
//				var newFileName = uniqueString + ext;
//				var fileWithPath = Path.Combine(path, newFileName);
//				var stream = new FileStream(fileWithPath, FileMode.Create);
//				imageFile.CopyTo(stream);
//				stream.Close();
//				return new Tuple<int, string>(1, newFileName);
//			}
//			catch (Exception ex)
//			{
//				return new Tuple<int, string>(0, "Error has occured");
//			}
//		}

//		public bool DeleteImage(string imageFileName)
//		{
//			try
//			{
//				var wwwPath = this.environment.WebRootPath;
//				var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
//				if (System.IO.File.Exists(path))
//				{
//					System.IO.File.Delete(path);
//					return true;
//				}
//				return false;
//			}
//			catch (Exception ex)
//			{
//				return false;
//			}
//		}
//	}
//}



using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Post;
using Infrastructure.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Implementation.Services;

public class FileService : IFileService
{
	private readonly IWebHostBuilder environment;
	private readonly IWebHostEnvironment _webHostEnvironment;
	private readonly FileSettingConfig _fileSettingConfig;
	private readonly IGenericRepository<Photo> _photoRepository;

	public FileService(IWebHostEnvironment webHostEnvironment, FileSettingConfig fileSettingConfig, IGenericRepository<Photo> photoRepository)
	{
		_webHostEnvironment = webHostEnvironment;
		_fileSettingConfig = fileSettingConfig;
		_photoRepository = photoRepository;
	}


	public async Task<string> UploadUserFile(IFormFile file, string uploadImageFolderPath)
	{
		var result = "";

		var wwwRootPath = _webHostEnvironment.WebRootPath;

		var imagePath = Path.Combine(wwwRootPath, uploadImageFolderPath);

		if (!Directory.Exists(imagePath))
		{
			Directory.CreateDirectory(imagePath);
		}

		var identifier = GenerateRandomWord(4);

		var renamedFileName = $"[{identifier}]{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}";

		var fileName = $"{renamedFileName}{Path.GetExtension(file.FileName)}";

		await using var stream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);

		await file.CopyToAsync(stream);

		result = Path.Combine(uploadImageFolderPath, fileName);

		return result;
	}

	private static string GenerateRandomWord(int length)
	{
		const string chars = "abcdefghijklmnopqrstuvwxyz";
		Random random = new Random();
		char[] word = new char[length];
		for (int i = 0; i < length; i++)
		{
			word[i] = chars[random.Next(chars.Length)];
		}
		return new string(word);
	}

	public async Task DeleteUserUpload(int id)
	{
		var photoToDelete = await _photoRepository.GetByIdAsync(id);
		if (photoToDelete != null)
		{
			photoToDelete.IsDeleted = true;
			photoToDelete.DeletedAt = DateTime.Now;
		}
		await _photoRepository.UpdateAsync(photoToDelete);
	}
}
