using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using Cleave.Middleware.Authentication.Models;
using System.Text.Json;

namespace Cleave.Middleware.Authentication
{
    public class CleaveAuthentication : IMiddleware
    {
        string jwtSecretKey = "kAyZgfQEmbf0aVWqfJYKs3zEpRctbgSBQRJ0OkzlDGsr0hI7eK0WtQjCr8IZOjoC";
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            IHeaderDictionary headers = context.Request.Headers;

            //JWT tokens
            if (headers.ContainsKey("Authorization") && headers["Authorization"].ToString().StartsWith("Bearer", StringComparison.InvariantCultureIgnoreCase))
            {
                bool isTokenValid = false;
                JwtAuthenticationResponse response;
                try
                {
                    isTokenValid = ValidateJwtToken(headers["Authorization"].ToString());
                }
                catch(SecurityTokenMalformedException ex) {
                    response = new JwtAuthenticationResponse("Token is Malformed");
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
                catch(SecurityTokenExpiredException ex)
                {
                    response = new JwtAuthenticationResponse("Token has been expired");
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
                catch(SecurityTokenInvalidSignatureException ex)
                {
                    response = new JwtAuthenticationResponse("Token Signature is not valid");
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
                catch(Exception ex)
                {
                    throw;
                }

                if (isTokenValid)
                {
                    await next(context);
                }
            }   
        }

        bool ValidateJwtToken(string token)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = "localhost",
                ValidAudience = "localhost",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
            };
            string jwtToken = token.Substring(7);
            IPrincipal principal = jwtSecurityTokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var validatedToken);
            Thread.CurrentPrincipal = principal;
            return true;
        }
    }
    public static class CleaveAuthenticationExtension
    {
        public static IApplicationBuilder UseCleaveAuthentication(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CleaveAuthentication>();
        }
    }
}
