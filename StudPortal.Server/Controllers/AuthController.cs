using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using StudPortal.Server.Models;
using StudPortal.Server.ViewModels;

namespace StudPortal.Server.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static Account acc = new Account();
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Account>> Login(AccountViewModel request)
        {
            if(acc.Username != request.Username)
            {
                return BadRequest("Username not found!");
            }

            if(!VerifyPasswordHash(request.Password, acc.PasswordHash, acc.PasswordSalt))
            {
                return BadRequest("Wrong password!");
            }

            string token = CreateToken(acc);

            //var refreshToken = GenerateRefreshToken();
            //SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Account>> Register(AccountViewModel request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            acc.Username = request.Username;
            acc.PasswordHash = passwordHash;
            acc.PasswordSalt = passwordSalt;

            return Ok(acc);
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                // A random string is added for endcrypting password
                passwordSalt = hmac.Key;

                // Encrypte password
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                // Hashing requested password
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                // Compare with stored hashpassword
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private byte[] GenerateRandomKey(int sizeInBits)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var key = new byte[sizeInBits / 8];
                rng.GetBytes(key);
                return key;
            }
        }

        private string CreateToken(Account acc)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, acc.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(GenerateRandomKey(512));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
