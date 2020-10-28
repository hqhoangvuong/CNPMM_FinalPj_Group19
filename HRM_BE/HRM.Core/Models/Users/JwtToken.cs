using System;

namespace HRM.Core.Models.Users
{
    public class JwtToken
    {
        public string Id { get; set; }
        public string TokenType { get; set; } = "JWT Bearer";
        public string AccessToken { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
