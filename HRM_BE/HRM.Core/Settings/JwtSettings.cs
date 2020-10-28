using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Core.Settings
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public double Expiration { get; set; }
    }
}
