using Application.DTOs.Event;
using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<IEnumerable<UserResponseDTO>> GetAllUsers(string rolename);

        Task<IEnumerable<DashboardEventResponseDTO>> GetEventDetails();
    }
}
