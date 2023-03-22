using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RecSysApi.Application.Commons.Settings;
using RecSysApi.Application.Interfaces.Update;
using RecSysApi.Domain.Constants;
using RecSysApi.Domain.Dtos.VideoUpvResponseDtos;
using RecSysApi.Domain.Entities;
using RecSysApi.Domain.Interfaces.Services;
using RecSysApi.Domain.Models;

namespace RecSysApi.Application.Servants;

public sealed class UpdateServant : IUpdateServant
{
    private readonly IDigestAuthService _digestAuthService;
    private readonly IHttpService _httpService;
    private readonly UpvSettings _upvSettings;
    private readonly TagsSettings _tagsSettings;

    public UpdateServant(IOptions<UpvSettings> upvSettings,
        IOptions<TagsSettings> tagsSettings,
        IHttpService httpService,
        IDigestAuthService digestAuthService)
    {
        _upvSettings = upvSettings.Value;
        _tagsSettings = tagsSettings.Value;
        _httpService = httpService;
        _digestAuthService = digestAuthService;
    }

    public async Task<Video> AddTranscription(Video video)
    {
        try
        {
            var hasTranscriptionsRequestUrl = new RequestUrl<string>
            {
                Protocol = _upvSettings.Protocol,
                Domain = _upvSettings.Domain,
                Path = "boguspath" + video.ExternalId.ToString().ToLowerInvariant()
            };

            var hasTranscriptionsResponse = await _httpService.SendGetRequestToApiAsync(hasTranscriptionsRequestUrl);
            if (hasTranscriptionsResponse is null || !hasTranscriptionsResponse.IsSuccessStatusCode) return video;

            var hasTranscriptionsResponseContent = await hasTranscriptionsResponse.Content.ReadAsStringAsync();
            var hasTranscriptionsResult =
                JsonConvert.DeserializeObject<List<UpvHasTranscriptionsResponse>>(hasTranscriptionsResponseContent);
            if (hasTranscriptionsResult?.FirstOrDefault(x => x.Lang == "es") is null) return video;

            var transcriptionRequestUrl = new RequestUrl<string>
            {
                Protocol = _upvSettings.Protocol,
                Domain = _upvSettings.Domain,
                Path = "boguspath"
            };

            var transcriptionResponse = await _httpService.SendGetRequestToApiAsync(transcriptionRequestUrl);
            var transcriptionResponseContent = await transcriptionResponse.Content.ReadAsStringAsync();
            var transcriptionResponseResult = transcriptionResponseContent
                .Split('\n')
                .Where((x, i) => (i + 2) % 4 == 0)
                .ToList();
            var transcription = string.Join(" ", transcriptionResponseResult.Where(s => !string.IsNullOrEmpty(s)));
            if (string.IsNullOrEmpty(transcription))
                return video;

            video.Transcription = transcription.Replace("  ", " ");
            video.HasTranscription = true;
            return video;
        }
        catch
        {
            return video;
        }
    }

    public async Task<CustomResponse<List<VideoUpvResponseDto>>> GetFromToVideos(DateTime fromDate, DateTime toDate)
    {
        var requestUrl = new RequestUrl<string>
        {
            Protocol = _upvSettings.Protocol,
            Domain = _upvSettings.Domain,
            Path = "bogus-path"
        };

        var response =
            await _digestAuthService.SendGetRequestToUpvAsync(requestUrl, _upvSettings.Username, _upvSettings.Password);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<VideoUpvResponseDto>>(content);

        return new CustomResponse<List<VideoUpvResponseDto>>
        {
            Status = HttpStatusCode.OK,
            Content = result
        };
    }

    public async Task<CustomResponse<string>> ComputeKeywords(string transcript)
    {
        var base64BasicAuth =
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_tagsSettings.Username}:{_tagsSettings.Password}"));

        var requestUrl = new RequestUrl<Dictionary<string, string>>
        {
            Protocol = "http",
            Domain = Constants.TagsAppenderDomain,
            Path = "boguspath"
        };
        requestUrl.Headers.Add(HttpRequestHeader.Authorization.ToString(), "Basic " + base64BasicAuth);
        requestUrl.Headers.Add(HttpRequestHeader.ContentType.ToString(), "application/json");
        requestUrl.Content = new Dictionary<string, string> {{"transcript", transcript }};

        var response = await _httpService.SendGetRequestToApiAsync(requestUrl);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var videosResponse = new CustomResponse<string>
            {
                Id = Guid.NewGuid(),
                Status = HttpStatusCode.OK,
                Content = content
            };
            return videosResponse;
        }

        return new CustomResponse<string>
        {
            Id = Guid.NewGuid(),
            Status = response.StatusCode
        };
    }
}