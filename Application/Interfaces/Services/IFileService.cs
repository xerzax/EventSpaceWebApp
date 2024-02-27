using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
	public interface IFileService
	{
		/*Tuple<int, string> SaveImage(IFormFile imageFile);
		public bool DeleteImage(string imageFileName);*/

		Task<string> UploadUserFile(IFormFile file, string uploadImageFolderPath);
		Task DeleteUserUpload(int id);
	}
}
