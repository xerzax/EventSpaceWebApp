using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Post
{
    public class UploadRequestDTO
    {

        public string FilePath { get; set; }

        public IFormFile File { get; set; }

    }
}
