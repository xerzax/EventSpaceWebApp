using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Identity;
using System.Threading.Tasks;
using Application.DTOs.Identity;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
	private readonly IUserIdentityService _userIdentityService;

	public AccountController(IUserIdentityService userIdentityService)
	{
		_userIdentityService = userIdentityService;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterDto register)
	{
		var result = await _userIdentityService.Register(register);

		if (string.IsNullOrEmpty(result.Item1) || string.IsNullOrEmpty(result.Item2))
		{
			return BadRequest("Registration failed.");
		}

		// You might want to return the user ID and code for email confirmation as part of the response
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

		return result switch
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
		var result = await _userIdentityService.ResetPassword(resetPassword);

		if (string.IsNullOrEmpty(result))
		{
			return BadRequest("Password reset failed.");
		}

		return Ok("Password has been reset successfully.");
	}
}
