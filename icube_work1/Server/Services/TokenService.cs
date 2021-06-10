using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Server.DTOs;
using Server.Interfaces;
using Server.Models;

namespace Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly string _clientId;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            _clientId = config["ClientId"];
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim("FactoryId", user.FactoryId.ToString()),
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public TokenResponseDto CreateAccessToken(User user,string refreshToken)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim("FactoryId", user.FactoryId.ToString()),
                new Claim("LoggedOn", DateTime.UtcNow.ToString())
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var encodedToken = tokenHandler.WriteToken(token);

            return new TokenResponseDto()
            {
                access_token = encodedToken,
                expiration = token.ValidTo,
                refresh_token = refreshToken,
                username = user.Username
            };
        }
        public RefreshToken CreateRefreshToken(int userId)
        {
            using(var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken()
                {
                    ClientId = _clientId,
                    UserId = userId,
                    Value = Convert.ToBase64String(randomBytes),
                    CreatedDate = DateTime.UtcNow,
                    ExpiresTime = DateTime.UtcNow.AddMinutes(90)
                };
            }

        }



    }
}