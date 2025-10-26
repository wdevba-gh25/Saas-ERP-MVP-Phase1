using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SaaSMvpContext _db;
        private readonly IConfiguration _config;

        public AuthController(SaaSMvpContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // POST: /api/auth/signup
        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupRequest req)
        {
            try
            {
                if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                    return BadRequest("User with this email already exists.");

                // Force all new users into the known existing Organization
                var fixedOrgId = Guid.Parse("D2F3B5C1-7E4A-4A1A-91B8-5C7F21E2E3AB");

                var hash = BCrypt.Net.BCrypt.HashPassword(req.Password);

                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    OrganizationId = fixedOrgId,
                    Email = req.Email,
                    PasswordHash = hash,
                    DisplayName = req.DisplayName
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                var token = GenerateJwt(user);
                Console.WriteLine($"[DEBUG] Created user {user.Email} with token: {token}");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SIGNUP ERROR] {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, "An error occurred during registration.");
            }
        }

        // POST: /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
                if (user == null)
                {
                    Console.WriteLine($"[LOGIN FAIL] User not found: {req.Email}");
                    return Unauthorized("Invalid credentials.");
                }

                var ok = BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash);

                // --- Legacy fallback (plaintext passwords) ---
                if (!ok && user.PasswordHash == req.Password)
                {
                    Console.WriteLine($"[LOGIN LEGACY] Upgrading plaintext password for {user.Email}");
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password);
                    await _db.SaveChangesAsync();
                    ok = true;
                }

                if (!ok)
                {
                    Console.WriteLine($"[LOGIN FAIL] Wrong password for {user.Email}");
                    return Unauthorized("Invalid credentials.");
                }

                var token = GenerateJwt(user);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOGIN ERROR] {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, "An error occurred during login.");
            }
        }

        private string GenerateJwt(User user)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("userId", user.UserId.ToString()),
                new Claim("organizationId", user.OrganizationId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiresMinutes"] ?? "60")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // -------------------------
    // Request DTOs
    // -------------------------
    public class SignupRequest
    {
        public Guid OrganizationId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}