
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Server.Controllers
{
    public class OAuthController: Controller
    {
        // ย้าย business logic ใน controller ต่างๆ ไปสร้าง repository
        [HttpGet]
        public IActionResult Authorize(
            string response_type,
            string client_id,
            string redirect_uri,
            string scope,
            string state)
        {
            var query = new QueryBuilder();
            query.Add("redirectUri",redirect_uri);
            query.Add("state",state);
            return View(model:query.ToString());
        }

        [HttpPost]
        public IActionResult Authorize(
            string username,
            string password,
            string redirectUri,
            string state)
        {
            const string code = "Authorization_code";
            var query = new QueryBuilder();
            query.Add("code",code);
            query.Add("state",state);

            return Redirect($"{redirectUri}{query.ToString()}");
        }

        public async Task<IActionResult> Token(
            string grant_type, // flow of access_token request
            string code, // confirmation of the authentication process
            string redirect_uri,
            string client_id)
        {
            // some mechanism for validating the code

            var secretBytes = Encoding.UTF8.GetBytes("super secret unguessable key");
            var key = new SymmetricSecurityKey(secretBytes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, "max")
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(3),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var access_token = tokenHandler.WriteToken(token);

            var responseObject = new
            {
                access_token,
                token_type = "Bearer",
            };

            var responseJson = JsonConvert.SerializeObject(responseObject);
            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            await Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);

            return Redirect(redirect_uri);
        }
        
    }
}
