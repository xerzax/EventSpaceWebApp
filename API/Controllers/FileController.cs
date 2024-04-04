using Application.DTOs;
using Application.DTOs.Post;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] UploadRequestDTO uploads)
        {
            if (uploads == null || uploads.File == null)
            {
                return BadRequest("Files are not provided.");
            }

            if (!int.TryParse(uploads.FilePath, out int filePathIndex))
            {
                return BadRequest("Invalid filePath parameter.");
            }

            var filePaths = filePathIndex switch
            {
                1 => "blog",
                2 => "event",
                3 => "post",
                _ => "images"
            };

            if (string.IsNullOrEmpty(filePaths))
            {
                return BadRequest("Invalid filePath index.");
            }

            var result = await _fileService.UploadUserFile(uploads.File, filePaths);
            
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            await _fileService.DeleteUserUpload(id);

            return Ok("File deleted successfully.");
        }
    }
}







