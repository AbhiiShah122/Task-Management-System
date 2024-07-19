using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Configuration;
using TaskManagementSystem.Data;
using TaskManagementSystem.Model.Response;
using TaskManagementSystem.Repositories.Interfaces;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Services.Implementations
{
    public class JWTService : IJWTService
    {
        private readonly IRoleRepository _roleRepository;
        public JwtSettings JwtSettings { get; set; }

        public JWTService(IOptionsMonitor<JwtSettings> jwtSettings, IRoleRepository roleRepository)
        {
            JwtSettings = jwtSettings.CurrentValue;
            _roleRepository = roleRepository;
        }

        public string GenerateToken(string email, string password)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(JwtSettings.Key);
                var roleId = GetRoleId(email, password) ?? throw new Exception("User not found");
                var userRole = GetRoleName(roleId) ?? throw new Exception("Role not found");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, userRole)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = JwtSettings.Issuer,
                    Audience = JwtSettings.Audience,
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while generating the token: {ex.Message}");
                throw;
            }
        }

        private int? GetRoleId(string email, string password)
        {
            return _roleRepository.GetRoleId(email, password);
        }


        private string? GetRoleName(int roleId)
        {
            return _roleRepository.GetRoleById(roleId);
        }
    }
}