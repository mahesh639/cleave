using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleave.ActionFilter.Authorization.Models
{
    public class UnAuthorizedResponse
    {
        public UnAuthorizedResponse(string reason, string authentication = "Failed") {
            this.Authentication = authentication;
            this.Reason = reason;
        }

        public string Authentication { get; set; }
        public string Reason { get; set; }
    }
}
