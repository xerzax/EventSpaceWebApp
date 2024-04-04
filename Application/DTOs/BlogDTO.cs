using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class BlogDTO
	{
        public string Title { get; set; }
        public string Content { get; set; }
        public string PhotoName { get; set; }
    }
}
