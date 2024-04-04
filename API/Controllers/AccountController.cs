using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Identity;
using System.Threading.Tasks;
using Application.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using Application.Interfaces.Services;
using Application.DTOs.Email;
using System.Security.Policy;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
	private readonly IUserIdentityService _userIdentityService;
	private readonly IEmailService _emailSender;


	public AccountController(IUserIdentityService userIdentityService, IEmailService emailSender = null)
	{
		_userIdentityService = userIdentityService;
		_emailSender = emailSender;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterDto register)
	{
		var result = await _userIdentityService.Register(register);
		
		//var url = Url.Link("ConfirmEmail", new { userId = result.Item1, code = result.Item2 });
		var url = Url.Action("ConfirmEmail", "Account", new { userId = result.Item1, code = result.Item2 }, Request.Scheme);


		EmailActionDto email = new EmailActionDto()
		{
			Email = register.Email,
			Subject = "Welcome To EventSpace",
			Url = url
		};
		await _emailSender.SendEmail(email);


		if (string.IsNullOrEmpty(result.Item1) || string.IsNullOrEmpty(result.Item2))
		{
			return BadRequest("Registration failed.");
		}

		return Ok(new { UserId = result.Item1, Code = result.Item2 });
	}

	

	[HttpGet("confirm-email")]
	public async Task<IActionResult> ConfirmEmail([FromQuery] Guid userId, [FromQuery] string code)
	{
		var result = await _userIdentityService.ConfirmEmail(userId, code);

		if (!result)
		{
			return BadRequest("Email confirmation failed.");
		}

		return Ok("Email successfully confirmed.");
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto login)
	{
		var result = await _userIdentityService.Login(login);
		if (result.Message == "Success")
		{
			//var token = _userIdentityService.GenerateTokenString(result.User);
			return Ok(new { Token = result.Token, Message = result.Message });
		}
		
		return result.Message switch
		{
			"Locked" => BadRequest("Account is locked."),
			"Invalid" => Unauthorized("Invalid login attempt."),
			"Success" => Ok("Login successful."),
			_ => BadRequest("Login failed.")
		};
	}

	[HttpPost("logout")]
	public async Task<IActionResult> Logout()
	{
		await _userIdentityService.LogOut();
		return Ok("Logged out successfully.");
	}

	[HttpPost("forgot-password")]
	public async Task<IActionResult> ForgetPassword([FromBody] ForgotPasswordDto forgotPassword)
	{
		var result = await _userIdentityService.ForgetPassword(forgotPassword);

		if (string.IsNullOrEmpty(result.Item1) || string.IsNullOrEmpty(result.Item2))
		{
			return BadRequest("Error generating password reset.");
		}

		// You might want to return the user ID and code for password reset
		return Ok(new { UserId = result.Item1, Code = result.Item2 });
	}

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userIdentityService.ResetPassword(resetPassword);

        if (string.IsNullOrEmpty(result))
        {
            return BadRequest("Password reset failed.");
        }

        return Ok("Password has been reset successfully.");
    }

}
