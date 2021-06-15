using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Interfaces;
using Server.Models;

namespace Server.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // ย้าย business logic ทั้งหมดไปเขียนไว้ใน AccountRepository
            // ใช้หลักการ DI -> IAccountRepository accountRepository -> _accountRepository = accountRepository (ใน Constructor)
            
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var factory = await _context.Factorys.FirstOrDefaultAsync(x => x.Factoryname == registerDto.Factoryname);

            if (factory== null) return BadRequest("Invalid Factory");

            var user = new User
            {
                Username = registerDto.Username.ToLower(),
                Passwordhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                Passwordsalt = hmac.Key,
                FactoryId = factory.FactoryId
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.Username,
                FactoryId = user.FactoryId,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // ย้าย business logic ทั้งหมดไปเขียนไว้ใน AccountRepository
            
            var user = await _context.Users.SingleOrDefaultAsync(x => (x.Username == loginDto.Username.ToLower())&&(x.Factory.Factoryname == loginDto.Factoryname.ToLower()));

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.Passwordsalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.Passwordhash[i]) return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                Username = user.Username,
                FactoryId = user.FactoryId,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            // ย้าย business logic ทั้งหมดไปเขียนไว้ใน AccountRepository
            return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
        }

    }
}
