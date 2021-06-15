using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Data;
using Server.DTOs;
using Server.Interfaces;

namespace Server.Controllers
{
    public class TokenController : ApiControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly string _clientId;

        public TokenController(DataContext context,IConfiguration config,ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
            _clientId = config["ClientId"];
        }
           
        // ย้าย business logic ใน controller ต่างๆ ไปสร้าง repository  
        
        [HttpPost("[action]")]
        public async Task<IActionResult> Auth(TokenRequestDto request)
        {

            if (request == null) return new StatusCodeResult(500);

            switch(request.GrantType)
                {
                case "password":
                    return await GenerateNewToken(request);
                case "refresh_token":
                    return await RefreshToken(request);
                default:
                    return new UnauthorizedResult();
            }

        }
        private async Task<IActionResult> GenerateNewToken(TokenRequestDto request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => (x.Username == request.Username.ToLower())&&(x.FactoryId == request.FactoryId));

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.Passwordsalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.Passwordhash[i]) return Unauthorized("Invalid password");
            }

            var newrefreshtoken = _tokenService.CreateRefreshToken(user.Id);

            var oldrefreshTokens = _context.RefreshTokens.Where(x => x.UserId == user.Id && x.Revoked == null);

            if (oldrefreshTokens != null)
                {
                    foreach (var oldtoken in oldrefreshTokens)
                    {
                        oldtoken.Revoked = DateTime.UtcNow;
                        oldtoken.ReplacedByToken = newrefreshtoken.Value;
                        _context.Update(oldtoken);
                    }
                }

            _context.RefreshTokens.Add(newrefreshtoken);
            await _context.SaveChangesAsync();

            var accessToken = _tokenService.CreateAccessToken(user, newrefreshtoken.Value);

            return Ok(new {authToken = accessToken });

        }

        private async Task<IActionResult> RefreshToken(TokenRequestDto request)
        {
            try
            {
                var refreshtoken = _context.RefreshTokens
                    .FirstOrDefault(x =>
                    x.ClientId == _clientId
                    && x.Value == request.RefreshToken.ToString());

                if (refreshtoken == null) return new UnauthorizedResult();

                if (!refreshtoken.IsActive) return new UnauthorizedResult();

                var user = await _context.Users.SingleOrDefaultAsync(x=>x.Id==refreshtoken.UserId);

                if (user == null) return new UnauthorizedResult();

                var Newrefreshtoken = _tokenService.CreateRefreshToken(user.Id);

                refreshtoken.Revoked = DateTime.UtcNow;
                refreshtoken.ReplacedByToken = Newrefreshtoken.Value;

                _context.RefreshTokens.Add(Newrefreshtoken);

                _context.Update(refreshtoken);

                await _context.SaveChangesAsync();

                var accesstoken = _tokenService.CreateAccessToken(user, Newrefreshtoken.Value);

                return Ok(new { authToken = accesstoken });

            }
            catch (Exception)
            {
                return new UnauthorizedResult();
            }
        }


    }
}
