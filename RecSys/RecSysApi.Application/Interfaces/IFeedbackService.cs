using System.Threading.Tasks;
using RecSysApi.Domain.Dtos.FeedbackDtos;
using RecSysApi.Domain.Entities;

namespace RecSysApi.Application.Interfaces;

public interface IFeedbackService
{
    Task<CustomResponse<string>> AddFeedback(AddFeedbackDto addFeedbackDto);

    Task<CustomResponse<GetFeedbacksByQueryIdResponseDto>> GetFeedbacksByQueryId(
        GetFeedbacksByQueryIdDto getFeedbacksByQueryIdDto);

    Task<CustomResponse<GetFeedbacksByVideoIdResponseDto>> GetFeedbacksByVideoId(
        GetFeedbacksByVideoIdDto getFeedbacksByVideoIdDto);
}