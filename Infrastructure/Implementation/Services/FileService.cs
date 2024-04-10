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

		var uploadsPath = Path.Combine(wwwRootPath, "uploads");

		var imagePath = Path.Combine(uploadsPath, uploadImageFolderPath);

		if (!Directory.Exists(imagePath))
		{
			Directory.CreateDirectory(imagePath);
		}

		var identifier = GenerateRandomWord(4);

		var renamedFileName = $"[{identifier}]{DateTime.Now:yyyyMMddHHmmssfff}";

		var fileName = $"{renamedFileName}{Path.GetExtension(file.FileName)}";

		await using var stream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);

		await file.CopyToAsync(stream);

		return fileName;
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
