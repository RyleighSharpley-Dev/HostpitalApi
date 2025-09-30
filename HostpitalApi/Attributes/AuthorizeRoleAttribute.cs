using HospitalApi.Data;
using HospitalApi.Models;
using HostpitalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace HospitalApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _allowedRoles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            _allowedRoles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var headers = context.HttpContext.Request.Headers;

            if (!headers.TryGetValue("x-api-key", out var givenApiKey))
            {
                context.Result = new JsonResult(new { message = "API Key missing or invalid" })
                { StatusCode = 401 };
                return;
            }

            // Convert StringValues to string
            var apiKey = givenApiKey.ToString();

            var dbContext = (ApplicationDbContext)context.HttpContext.RequestServices
                .GetService(typeof(ApplicationDbContext));

            // Look up user by API key
            var user = dbContext.Admins.FirstOrDefault(a => a.apiKey == apiKey)
                       ?? (object)dbContext.Doctors.FirstOrDefault(d => d.apiKey == apiKey)
                       ?? dbContext.Patients.FirstOrDefault(p => p.apiKey == apiKey);

            if (user == null)
            {
                context.Result = new JsonResult(new { message = "The provided API Key is invalid" })
                { StatusCode = 401 };
                return;
            }

            // Determine role
            string userRole = (user is Admin a) ? a.Role :
                              (user is Doctor d) ? d.Role :
                              (user is Patient p) ? p.Role :
                              string.Empty;

            if (!_allowedRoles.Contains(userRole))
            {
                context.Result = new JsonResult(new { message = "Forbidden: Role not authorized" })
                { StatusCode = 403 };
                return;
            }

            // Attach user info to HttpContext for later use
            context.HttpContext.Items["UserRole"] = userRole;
            context.HttpContext.Items["User"] = user;
        }
    }
}
