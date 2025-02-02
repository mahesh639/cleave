using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleave.Middleware.Authentication.Models
{
    internal class JwtAuthenticationResponse
    {
        public JwtAuthenticationResponse(string reason, string authentication = "Failed") { 
            Reason = reason;
            Authentication = authentication;
        }
        public string Authentication { get; set; }
        public string Reason { get; set; }
    }
}
