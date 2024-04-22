using Application.DTOs;
using Application.DTOs.Event_DTO;
using Application.DTOs.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IDonationService
    {
        Task<DonationResponseDTO> CreateDonation(DonationRequestDTO evt);

    }
}
