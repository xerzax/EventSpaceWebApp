using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User
{
    public class UserResponseDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }


        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Contact { get; set; }
        public bool? IsConfirmed { get; set; }


    }
}
