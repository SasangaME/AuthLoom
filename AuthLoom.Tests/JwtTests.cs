using AuthLoom.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLoom.Tests
{

    public class JwtTests
    {
        [Fact]
        public void CreateTokenWhenParamsSupplied()
        {
            string secret = "nFd9lguv5L2PIdNRiqy4g5XZ7aCVJs6D";
            string username = "sasanga";
            string userId = "1";
            string role = "user";
            string issuer = "localhost";
            string audience = "localhost";
            var result = JwtUtil.CreateToken(secret: secret, userId: userId, username: username, role: role, issuer: issuer, audience: audience);
            Assert.NotNull(result);
        }
    }
}
