using AutoMapper;
using FluentValidation.Results;
using HealthCheck.Authentication.Configuration;
using HealthCheck.Authentication.Models.DTOs.Incoming;
using HealthCheck.Authentication.Models.DTOs.Outgoing;
using HealthCheck.DataService.IConfiguration;
using HealthCheck.Entities.DbSet;
using HealthCheck.WebApi.Controllers.v1.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HealthCheck.WebApi.Controllers.v1
{
  
    public class AccountsController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameter;
        private readonly JwtConfig _jwtConfig;

        public AccountsController(IUnitOfWork unit, IMapper mapper
            ,UserManager<IdentityUser> userManager
            ,IOptionsMonitor<JwtConfig> optionsMonitor
            ,TokenValidationParameters tokenValidationParameter) : base(unit, mapper)
        {
            _userManager = userManager;
            _tokenValidationParameter = tokenValidationParameter;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationRequestDto dto)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null)
            {
                return BadRequest(new UserRegistrationResponseDto()
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "User with this email already exists"
                    }

                });
            }
            user = new()
            {
                Email = dto.Email,
                UserName = dto.Firstname,
                EmailConfirmed = true
            };

            IdentityResult isCreated = await _userManager.CreateAsync(user, dto.Password);

            if (!isCreated.Succeeded) return BadRequest(new UserRegistrationResponseDto()
            {
                Success = isCreated.Succeeded,
                Errors = isCreated.Errors.Select(x => x.Description).ToList()
            });

            User newUser = new()
            {
                IdentityId = new Guid(user.Id),
                Email = dto.Email,
                Firstname = dto.Firstname,
                Lastname = dto.Lastname,
                Phone = "050-366-01-32",
                Country = "Aze",
                DateOfBirth = DateTime.Now,
                AddedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Status = 1
            };

            await _unit.Users.Add(newUser);
            await _unit.SaveChangesAsync();

            var token = GenerateJwtToken(user);
         


            return Ok(new UserRegistrationResponseDto()
            {
                Success=true,
                Token= await token
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequestDto dto)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return BadRequest(new UserLoginResponseDto()
            {
                Success = false,
                Errors = new List<string>()
                {
                    "Invalid email or password"
                }
            });
            bool result = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!result) return BadRequest(new UserLoginResponseDto()
            {
                Success = false,
                Errors = new List<string>()
                {
                    "Invalid email or password"
                }
            });

            var jwtToken = GenerateJwtToken(user);
            return Ok(new UserLoginResponseDto()
            {
                Success=true,
                Token= await jwtToken
            });
        }
        
        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),// refresh token uchun
                    
                }),
                Expires = DateTime.Now.Add(_jwtConfig.ExpiryTimeFrame),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key)
                , SecurityAlgorithms.HmacSha256)
            };

            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            //refresh token

            RefreshToken refreshToken = new RefreshToken
            {
                IsRevoked = false,
                IsUsed = false,
                ExpiryDate = DateTime.Now.AddMonths(6),
                JwtId = token.Id,
                UserId = user.Id,
                Status = 1,
                Token = $"{RandomStringGenerator(25)}_{Guid.NewGuid()}",
                AddedDate = DateTime.Now

            };

            await _unit.RefreshTokens.Add(refreshToken);
            await _unit.SaveChangesAsync();
            return jwtToken;
        }
        private string RandomStringGenerator(int length)
        {
            var random = new Random();
            const string chars = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());    
        }
    }

    
}
