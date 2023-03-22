using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecSysApi.Domain.Entities;

namespace RecSysApi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        var result = new CustomResponse<string>
        {
            Status = HttpStatusCode.OK,
            Content = "Healthy"
        };
        return StatusCode((int) result.Status, JsonConvert.SerializeObject(result));
    }
}