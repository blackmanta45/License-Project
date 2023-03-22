using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecSysApi.Application.Interfaces.Update;
using RecSysApi.Domain.Entities.WebApi.Entities;
using RecSysApi.Presentation.Authorization;

namespace RecSysApi.Presentation.Controllers;

[ApiController]
public class UpdateController : ControllerBase
{
    private readonly IUpdateService _updateService;

    public UpdateController(IUpdateService updateService)
    {
        _updateService = updateService;
    }

    [HttpGet]
    [Authorize]
    [Route("api/[controller]")]
    public async Task<IActionResult> UpdateVideos()
    {
        var result = await _updateService.UpdateVideos();
        return StatusCode((int) result.Status, JsonConvert.SerializeObject(result));
    }

    [HttpGet]
    [Authorize(Role = Role.Admin)]
    [Route("api/[controller]/all")]
    public async Task<IActionResult> UpdateAllVideos()
    {
        var start = DateTime.UnixEpoch.AddYears(31);
        var result = await _updateService.UpdateAllVideosSince(start);
        return StatusCode((int) result.Status, JsonConvert.SerializeObject(result));
    }
}