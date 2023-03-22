using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RecSysApi.Application.Commons.Settings;
using RecSysApi.Application.Interfaces;
using RecSysApi.Application.Interfaces.Settings;
using RecSysApi.Domain.Constants;
using RecSysApi.Domain.Dtos.SearchForQueryDtos;
using RecSysApi.Domain.Dtos.VideosQueryDtos;
using RecSysApi.Domain.Entities;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Domain.Interfaces.Services;

namespace RecSysApi.Application.Services;

public class SearchEngineService : ISearchEngineService
{
    private readonly IHttpService _httpService;
    private readonly ILogger<SearchEngineService> _logger;
    private readonly ISfqSettings _sfqSettings;
    private readonly IVideoRepository _videoRepository;

    public SearchEngineService(ILogger<SearchEngineService> logger,
        IHttpService httpService,
        IVideoRepository videoRepository,
        IOptions<SfqSettings> sfqSettings)
    {
        _logger = logger;
        _httpService = httpService;
        _videoRepository = videoRepository;
        _sfqSettings = sfqSettings.Value;
    }

    public async Task<CustomResponse<SearchEngineQueryPaginatedResponseDto>> SendQueryToSfq(
        GetVideosQueryPaginatedDto getVideosQueryPaginatedDto)
    {
        var base64BasicAuth =
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_sfqSettings.SfqUsername}:{_sfqSettings.SfqPassword}"));

        var requestUrl = new RequestUrl<string>
        {
            Protocol = "http",
            Domain = Constants.SearchForAQueryDomain,
            Path = "SFQ/paginated"
        };
        requestUrl.Headers.Add(HttpRequestHeader.Authorization.ToString(), "Basic " + base64BasicAuth);
        requestUrl.QueryParams.Add("query", getVideosQueryPaginatedDto.Query);
        requestUrl.QueryParams.Add("page", getVideosQueryPaginatedDto.Page);
        requestUrl.QueryParams.Add("chunk-size", getVideosQueryPaginatedDto.ChunkSize);

        var response = await _httpService.SendGetRequestToApiAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SearchEngineQueryPaginatedResponseDto>(content);
            var videosResponse = new CustomResponse<SearchEngineQueryPaginatedResponseDto>
            {
                Id = Guid.NewGuid(),
                Status = HttpStatusCode.OK,
                Content = result
            };
            return videosResponse;
        }

        return new CustomResponse<SearchEngineQueryPaginatedResponseDto>
        {
            Id = Guid.NewGuid(),
            Status = response.StatusCode
        };
    }
}