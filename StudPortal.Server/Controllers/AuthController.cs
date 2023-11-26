using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using StudPortal.Server.Models;
using StudPortal.Server.ViewModels;
using Microsoft.Extensions.Configuration;
using StudPortal.Server.Helpers;
using StudPortal.Server.Services.AccountService;

namespace StudPortal.Server.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static HashHelper _hashHelper;
        private readonly IAccountService _accountService;

        public AuthController(IConfiguration config, IAccountService accountService)
        {
            _accountService = accountService;
            _hashHelper = new HashHelper();
        }

        [HttpPost("login")]
        public async Task<ActionResult<Account>> Login(LoginViewModel request)
        {
            // Handle invalid model / invalid information
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Account loginAccount = _accountService.GetAccountByUsername(request.Username);

            // Handle username not exist
            if (loginAccount == null)
            {
                return BadRequest("Username not found!");
            }

            // Handle Wrong request password
            if(!_hashHelper.VerifyPasswordHash(request.Password, loginAccount.PasswordHash, loginAccount.PasswordSalt))
            {
                return BadRequest("Wrong password!");
            }

            // Generate token
            // This token hold account information(Id, full name, role)
            string token = _hashHelper.CreateToken(loginAccount);

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Account>> Register(RegisterViewModel request)
        {
            // Handle invalid model / invalid information
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Handle existed account case
            if (_accountService.GetAccountByUsername(request.Username) != null)
            {
                return BadRequest("Username existed!");
            }

            // Handle repeat password not match password case
            if (request.Password != request.RepeatPassword)
            {
                return BadRequest("Repeat Password must match your Password!");
            }
            
            // Hash Password, out passwordHash and passwordSalt 
            _hashHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Generate new account model
            Account registerAccount = new Account
            {
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User"
            };

            // Add new register account to database
            if (_accountService.Add(registerAccount))
            {
                return Ok(registerAccount);
            }
            else
            {
                return BadRequest("Cannot register!");
            }
        }

        [HttpGet("clean")]
        public async Task<ActionResult> Clean()
        {
            // Remove all existing accounts in database 
            _accountService.Clean();

            // Number of existing account after remove
            int numberOfAcc = _accountService.GetAccountNumber();

            return Ok("Number of exsiting account: " + numberOfAcc);
        }
    }
}
