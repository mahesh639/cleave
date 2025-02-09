using Cleave.ActionFilter.Authorization.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Cleave.ActionFilter.Authorization
{
    public class CleaveAuthorizeAttribute : Attribute, IActionFilter
    {
        readonly string role;

        public CleaveAuthorizeAttribute(string role)
        {
            this.role = role;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            ClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (claimsPrincipal == null || claimsPrincipal.FindFirst("Role") == null || claimsPrincipal.FindFirst("Role").Value != role)
            {
                context.Result = new ObjectResult(new UnAuthorizedResponse("User does not have proper authorization"))
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Post Execution of the Controller or EndPoint
        }
    }
}
