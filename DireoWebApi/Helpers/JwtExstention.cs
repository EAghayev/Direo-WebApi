using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DireoWebApi.Helpers
{
    public static class JwtExstention
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Expose-Header", "Application-Error");
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                return null; //throw new ArgumentNullException(nameof(principal));

            string userId = null;

            try
            {
                userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            catch (System.Exception)
            {
            }
            return userId;
        }
    }
}
