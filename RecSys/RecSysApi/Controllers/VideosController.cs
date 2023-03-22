using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecSysApi.Application.Interfaces.VideosLookup;
using RecSysApi.Domain.Dtos.VideosQueryDtos;
using RecSysApi.Presentation.Authorization;

namespace RecSysApi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VideosController : ControllerBase
{
    private readonly ILogger<VideosController> _logger;
    private readonly IVideosLookupService _videosLookupService;

    public VideosController(ILogger<VideosController> logger, IVideosLookupService videosLookupService)
    {
        _logger = logger;
        _videosLookupService = videosLookupService;
    }

    [HttpGet]
    public async Task<IActionResult> GetVideosForQuery(
        [FromQuery] GetVideosQueryPaginatedDto getVideosQueryPaginatedDto)
    {
        var queryResponse = await _videosLookupService.LookupForVideos(getVideosQueryPaginatedDto);
        return StatusCode((int) queryResponse.Status, JsonConvert.SerializeObject(queryResponse));
    }
}