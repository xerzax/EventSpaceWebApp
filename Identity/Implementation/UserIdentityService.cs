using Application.Interfaces.Identity;
using Application.DTOs.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventSpaceApi.Domain.Entity.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Application.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace Identity.Implementation
{
	public class UserIdentityService : IUserIdentityService
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly IConfiguration _config;
		private readonly IGenericRepository<User> _genericRepo;
		private readonly IHttpContextAccessor _httpContextAccessor;



		public UserIdentityService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IConfiguration config, IGenericRepository<User> genericRepo, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_config = config;
			_genericRepo = genericRepo;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<Tuple<string, string>> Register(RegisterDto register, string? returnUrl = null)
		{
			try
			{
				var user = new User()
				{
					Name = register.Name,
					UserName = register.Email,
					Email = register.Email,
				};

				var result = await _userManager.CreateAsync(user, register.Password);

				if (!result.Succeeded) return new Tuple<string, string>(string.Empty, string.Empty);

				// Check if the role exists
				var roleExists = await _roleManager.RoleExistsAsync(register.Role);
				if (roleExists)
				{
					await _userManager.AddToRoleAsync(user, register.Role);

				}

				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				//return new Tuple<string, string>(user.Id.ToString(), WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)));
				return new Tuple<string, string>(user.Id.ToString(), code);


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		

		public async Task<bool> ConfirmEmail(Guid userId, string code)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(userId.ToString());

				if (user == null)
				{
					return false;
				}

				var result = await _userManager.ConfirmEmailAsync(user, code);

				return result.Succeeded;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		//public async Task<User> FindUserByEmail(string email)
		//{
		//	try
		//	{
		//		var user = await _genericRepo.GetFirstOrDefault(x => x.Email == email);
		//		return user;
		//	}
		//	catch (Exception e)
		//	{

		//		throw e;

		//	}
		//}



			public async Task<LoginResponseDTO> Login(LoginDto login, string? returnUrl = null)
		{
			try
			{
				var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, lockoutOnFailure: true);
				var response = new LoginResponseDTO();
				if (!result.Succeeded)
				{
					response.Message = "Invalid login attempt.";
					return response;
				}

				if (result.Succeeded)
				{
					response.Message = "Success";
					var user = await _genericRepo.GetFirstOrDefault(x => x.Email == login.Email);
					if (user  !=null)
					{
						var token = GenerateTokenString(user);
						response.Token = token;
					}
				}

				if (result.IsLockedOut)
				{
					response.Message = "Locked out";
				}

				return response;
			

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public async Task LogOut()
		{
			try
			{
				await _signInManager.SignOutAsync();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public async Task<Tuple<string, string>> ForgetPassword(ForgotPasswordDto forgotPassword)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(forgotPassword.Email);

				if (user == null) return new Tuple<string, string>(string.Empty, string.Empty);

				var code = await _userManager.GeneratePasswordResetTokenAsync(user);

				return new Tuple<string, string>(user.Id.ToString(), WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)));
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public async Task<string> ResetPassword(ResetPasswordDto resetPassword)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(resetPassword.Email);

				if (user == null) return string.Empty;

				var result = await _userManager.ResetPasswordAsync(user, resetPassword.Code, resetPassword.Password);

				return result.Succeeded ? "Success" : string.Empty;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public string GenerateTokenString(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var keyApp = _config["Jwt:Key"];
			var key = Encoding.ASCII.GetBytes(keyApp);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.Name.ToString())
				}),
				Expires = DateTime.UtcNow.AddMinutes(60),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
				Audience = _config["Jwt:Audience"],
				Issuer = _config["Jwt:Issuer"]
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public UserContext GetLoggedInUser()
		{
			HttpContext httpContext = _httpContextAccessor?.HttpContext;
			if (httpContext != null && httpContext.User.Identity.IsAuthenticated)
			{
				ClaimsIdentity claimsIdentity = (ClaimsIdentity)httpContext.User.Identity;
				Claim claimId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
				Claim claimUserName = claimsIdentity.FindFirst(ClaimTypes.Name);
				var user = new UserContext()
				{
					UserId = Guid.Parse(claimId.Value),
					UserName = claimUserName.Value,
				};
				return user;
			}
			return null;
		}
	}


		//public string GenerateTokenString(string userId, string name)
		//{
		//	var claims = new List<Claim>
		//{
		//	new Claim(ClaimTypes.Email, user.Email),

		//          // Add more claims as needed
		//      };

		//	SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
		//	SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

		//	var securityToken = new JwtSecurityToken(
		//		issuer: _config["Jwt:Issuer"],
		//		audience: _config["Jwt:Audience"],
		//		claims: claims,
		//		expires: DateTime.Now.AddMinutes(60),
		//		signingCredentials: signingCred);

		//	string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
		//	return tokenString;
		//}
	}

