using AuthLoom.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthLoom.Jwt
{
    public class JwtUtil
    {
        public static string CreateToken(string secret, string userId, string username, string role,
            string issuer, string audience)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role)
            };
            var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims,
                expires: DateTime.Now.AddMinutes(150), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static JwtInfo GetTokenInfo(string token, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.UTF8.GetBytes(secret);
            try
            {

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwt = (JwtSecurityToken)validatedToken;
                var username = jwt.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
                var userId = jwt.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                var role = jwt.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;

                return new JwtInfo { UserId = int.Parse(userId), Username = username, Role = role };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
