using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RecSysApi.Application.Interfaces;
using RecSysApi.Application.Mappers;
using RecSysApi.Domain.Dtos.FeedbackDtos;
using RecSysApi.Domain.Entities;
using RecSysApi.Domain.Interfaces.Repositories;

namespace RecSysApi.Application.Services;

public sealed class FeedbackService : IFeedbackService
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IQueryRepository _queryRepository;
    private readonly IVideoRepository _videoRepository;

    public FeedbackService(IFeedbackRepository feedbackRepository,
        IQueryRepository queryRepository,
        IVideoRepository videoRepository)
    {
        _feedbackRepository = feedbackRepository;
        _queryRepository = queryRepository;
        _videoRepository = videoRepository;
    }

    public async Task<CustomResponse<string>> AddFeedback(AddFeedbackDto addFeedbackDto)
    {
        var query = await _queryRepository.GetByIdAsync(addFeedbackDto.QueryId);
        if (query is null)
            return new CustomResponse<string>
            {
                Status = HttpStatusCode.NotFound
            };

        var video = await _videoRepository.GetByExternalId(addFeedbackDto.VideoId);
        if (video is null)
            return new CustomResponse<string>
            {
                Status = HttpStatusCode.NotFound
            };

        var feedback = FeedbackMapper.FromDto(addFeedbackDto, query, video);

        await _feedbackRepository.AddAsync(feedback);

        return new CustomResponse<string>
        {
            Status = HttpStatusCode.OK
        };
    }

    public async Task<CustomResponse<GetFeedbacksByQueryIdResponseDto>> GetFeedbacksByQueryId(
        GetFeedbacksByQueryIdDto getFeedbacksByQueryIdDto)
    {
        var feedbacks = await _feedbackRepository.GetFeedbacksByQueryId(getFeedbacksByQueryIdDto.QueryId);
        if (!feedbacks.Any())
            return new CustomResponse<GetFeedbacksByQueryIdResponseDto>
            {
                Status = HttpStatusCode.NotFound
            };

        var feedbackDtos = feedbacks.Select(FeedbackMapper.ToDto).ToList();

        var getFeedbacksByQueryIdResponseDto = new GetFeedbacksByQueryIdResponseDto
        {
            FeedbackDtos = feedbackDtos
        };
        return new CustomResponse<GetFeedbacksByQueryIdResponseDto>
        {
            Status = HttpStatusCode.OK,
            Content = getFeedbacksByQueryIdResponseDto
        };
    }

    public async Task<CustomResponse<GetFeedbacksByVideoIdResponseDto>> GetFeedbacksByVideoId(
        GetFeedbacksByVideoIdDto getFeedbacksByVideoIdDto)
    {
        var internalId = await _videoRepository.GetInternalIdBasedOnExternalId(getFeedbacksByVideoIdDto.VideoId);
        var feedbacks = await _feedbackRepository.GetFeedbacksByVideoId(internalId);
        if (!feedbacks.Any())
            return new CustomResponse<GetFeedbacksByVideoIdResponseDto>
            {
                Status = HttpStatusCode.NotFound
            };

        var feedbackDtos = feedbacks.Select(FeedbackMapper.ToDto).ToList();

        var getFeedbacksByQueryIdResponseDto = new GetFeedbacksByVideoIdResponseDto
        {
            FeedbackDtos = feedbackDtos
        };
        return new CustomResponse<GetFeedbacksByVideoIdResponseDto>
        {
            Status = HttpStatusCode.OK,
            Content = getFeedbacksByQueryIdResponseDto
        };
    }
}