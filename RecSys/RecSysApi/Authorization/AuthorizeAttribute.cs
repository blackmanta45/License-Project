using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using RecSysApi.Domain.Entities;
using RecSysApi.Domain.Models;

namespace RecSysApi.Presentation.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public string Role { get; set; }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (User) context.HttpContext.Items["User"];
        if (user == null || (user.Role != Role && Role != null))
        {
            // not logged in
            var result = new CustomResponse<string>
            {
                Status = HttpStatusCode.Unauthorized,
                Content = "Unauthorized"
            };
            context.Result = new ObjectResult(JsonConvert.SerializeObject(result));
        }
    }
}