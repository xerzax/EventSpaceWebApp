using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Identity;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}