using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecSysApi.Application.Interfaces;
using RecSysApi.Domain.Dtos.QueryDtos;
using RecSysApi.Presentation.Authorization;

namespace RecSysApi.Presentation.Controllers;

[ApiController]
[Authorize]
public class QueriesController : ControllerBase
{
    private readonly ILogger<QueriesController> _logger;
    private readonly IQueriesService _queriesService;

    public QueriesController(ILogger<QueriesController> logger,
        IQueriesService queriesService)
    {
        _logger = logger;
        _queriesService = queriesService;
    }

    [HttpGet]
    [Route("api/[controller]/getById")]
    public async Task<IActionResult> GetQueryById(
        [FromQuery] GetQueryByIdDto getQueryByIdDto)
    {
        try
        {
            var queryResponse = await _queriesService.GetQueryById(getQueryByIdDto);
            return StatusCode((int)queryResponse.Status, JsonConvert.SerializeObject(queryResponse));
        }
        catch
        {
            return NotFound(404);
        }
    }

    [HttpGet]
    [Route("api/[controller]/getByUserId")]
    public async Task<IActionResult> GetQueriesForUserId(
        [FromQuery] GetQueriesForUserIdDto getQueriesForUserIdDto)
    {
        try
        {
            var queryResponse = await _queriesService.GetQueriesForUserId(getQueriesForUserIdDto);
            return StatusCode((int) queryResponse.Status, JsonConvert.SerializeObject(queryResponse));
        }
        catch
        {
            return NotFound(404);
        }
    }

    [HttpGet]
    [Route("api/[controller]/all")]
    public async Task<IActionResult> GetAllQueries()
    {
        try
        {
            var queryResponse = await _queriesService.GetAllQueries();
            return StatusCode((int) queryResponse.Status, JsonConvert.SerializeObject(queryResponse));
        }
        catch
        {
            return NotFound(404);
        }
    }
}