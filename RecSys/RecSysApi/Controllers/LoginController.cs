using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecSysApi.Application.Interfaces;
using RecSysApi.Domain.Entities;

namespace RecSysApi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLogin login)
    {
        var result = await _loginService.Authenticate(login);
        return StatusCode((int) result.Status, JsonConvert.SerializeObject(result));
    }
}