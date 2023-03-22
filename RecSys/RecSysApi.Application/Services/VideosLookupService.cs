using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RecSysApi.Application.Interfaces.VideosLookup;
using RecSysApi.Domain.Dtos.VideosQueryDtos;
using RecSysApi.Domain.Entities;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Domain.Interfaces.Services;

namespace RecSysApi.Application.Services;

public class VideosLookupService : IVideosLookupService
{
    private readonly IQueryRepository _queryRepository;
    private readonly ISearchEngineService _searchEngineService;
    private readonly IVideoRepository _videoRepository;
    private readonly IVideosLookupServant _videosLookupServant;
    private ILogger<VideosLookupService> _logger;

    public VideosLookupService(ILogger<VideosLookupService> logger,
        ISearchEngineService searchEngineService,
        IVideoRepository videoRepository,
        IVideosLookupServant videosLookupServant,
        IQueryRepository queryRepository)
    {
        _searchEngineService = searchEngineService;
        _videoRepository = videoRepository;
        _videosLookupServant = videosLookupServant;
        _queryRepository = queryRepository;
        _logger = logger;
    }

    public async Task<CustomResponse<GetVideosQueryPaginatedResponseDto>> LookupForVideos(
        GetVideosQueryPaginatedDto getVideosQueryPaginatedDto)
    {
        var getVideosQueryPaginatedResponseDto =
            await _searchEngineService.SendQueryToSfq(getVideosQueryPaginatedDto);

        if (getVideosQueryPaginatedResponseDto.Status != HttpStatusCode.OK)
            return new CustomResponse<GetVideosQueryPaginatedResponseDto>
            {
                Id = Guid.NewGuid(),
                Status = getVideosQueryPaginatedResponseDto.Status
            };

        var searchEngineQueryVideoPaginatedResponseDto = getVideosQueryPaginatedResponseDto.Content;
        var queryDto =
            _videosLookupServant.CreateQueryDto(getVideosQueryPaginatedDto, searchEngineQueryVideoPaginatedResponseDto);
        var query = await _queryRepository.AddQuery(queryDto);

        var internalVideosIds = searchEngineQueryVideoPaginatedResponseDto.Videos.Select(x => x.Id).ToList();
        var externalVideosIds = await _videoRepository.GetExternalIdsBasedOnInternalIds(internalVideosIds);

        var responseContent = new GetVideosQueryPaginatedResponseDto
        {
            QueryId = query.Id,
            TotalVideosForQuery = getVideosQueryPaginatedResponseDto.Content.TotalVideosForQuery,
            VideosIds = externalVideosIds
        };

        return new CustomResponse<GetVideosQueryPaginatedResponseDto>
        {
            Id = Guid.NewGuid(),
            Status = HttpStatusCode.OK,
            Content = responseContent
        };
    }
}