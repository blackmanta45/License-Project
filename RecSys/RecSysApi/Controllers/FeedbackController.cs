using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecSysApi.Application.Interfaces;
using RecSysApi.Domain.Dtos.FeedbackDtos;
using RecSysApi.Presentation.Authorization;

namespace RecSysApi.Presentation.Controllers;

[ApiController]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpPost]
    [Authorize]
    [Route("api/[controller]/add")]
    public async Task<IActionResult> AddFeedback([FromQuery] AddFeedbackDto addFeedbackDto)
    {
        var result = await _feedbackService.AddFeedback(addFeedbackDto);

        return StatusCode((int) result.Status, JsonConvert.SerializeObject(result));
    }

    [HttpGet]
    [Authorize]
    [Route("api/[controller]/getByQueryId")]
    public async Task<IActionResult> GetFeedbackByQueryId([FromQuery] GetFeedbacksByQueryIdDto getFeedbacksByQueryIdDto)
    {
        var result = await _feedbackService.GetFeedbacksByQueryId(getFeedbacksByQueryIdDto);

        return StatusCode((int) result.Status, JsonConvert.SerializeObject(result));
    }

    [HttpGet]
    [Authorize]
    [Route("api/[controller]/getByVideoId")]
    public async Task<IActionResult> GetFeedbackByQueryId([FromQuery] GetFeedbacksByVideoIdDto getFeedbacksByQueryIdDto)
    {
        var result = await _feedbackService.GetFeedbacksByVideoId(getFeedbacksByQueryIdDto);

        return StatusCode((int) result.Status, JsonConvert.SerializeObject(result));
    }
}