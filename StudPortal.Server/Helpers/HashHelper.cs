using Microsoft.IdentityModel.Tokens;
using StudPortal.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace StudPortal.Server.Helpers
{
    public class HashHelper
    {
        private readonly string _secretKey;
        public HashHelper()
        {
            _secretKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings:Token").Value;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                // A random string is added for endcrypting password
                passwordSalt = hmac.Key;

                // Encrypte password
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                // Hashing requested password
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                // Compare with stored hashpassword
                return computeHash.SequenceEqual(passwordHash);
            }
        }
        public byte[] GenerateRandomKey(int sizeInBits)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var key = new byte[sizeInBits / 8];
                rng.GetBytes(key);
                return key;
            }
        }

        public string CreateToken(Account acc)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, acc.Id.ToString()),
                new Claim(ClaimTypes.Name, acc.FirstName + " " + acc.LastName),
                new Claim(ClaimTypes.Role, acc.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_secretKey));

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
