using Server.DTOs;
using Server.Models;

namespace Server.Interfaces
{
    public interface ITokenService
    {
         string CreateToken(User user);
         RefreshToken CreateRefreshToken(int userId);
         TokenResponseDto CreateAccessToken(User user,string refreshToken);
    }
}