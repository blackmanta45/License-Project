using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RecSysApi.Application.Commons.Extensions;
using RecSysApi.Application.Interfaces.Update;
using RecSysApi.Application.Mappers;
using RecSysApi.Domain.Entities;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Domain.Models;

namespace RecSysApi.Application.Services;

public class UpdateService : IUpdateService
{
    private readonly IUpdateRepository _updateRepository;
    private readonly IUpdateServant _updateServant;
    private readonly IVideoRepository _videoRepository;

    public UpdateService(IVideoRepository videoRepository,
        IUpdateRepository updateRepository,
        IUpdateServant updateServant)
    {
        _videoRepository = videoRepository;
        _updateRepository = updateRepository;
        _updateServant = updateServant;
    }

    public async Task<CustomResponse<string>> UpdateVideos()
    {
        if (_videoRepository.GetCount() == 0)
        {
            var start = DateTime.UnixEpoch.AddYears(31);
            await UpdateAllVideosSince(start);
            return await UpdateKeywords();
        }

        await UpdateDbVideos();
        var lastUpdate = await _updateRepository.GetLastUpdateTime();
        var response = await UpdateAllVideosSince(lastUpdate);
        if (response.Status != HttpStatusCode.OK)
            return response;

        return await UpdateKeywords();
    }

    public async Task<CustomResponse<string>> UpdateKeywords()
    {
        var validVideosWithoutKeywords = await _videoRepository.GetValidVideosWithoutKeywords();

        foreach (var video in validVideosWithoutKeywords)
        {
            var response = await _updateServant.ComputeKeywords(video.Transcription);
            if (response.Content is null) continue;
            video.Keywords = response.Content;
            video.ModifiedAt = DateTime.Now;
            await _videoRepository.UpdateAsync(video);
        }

        return new CustomResponse<string>
        {
            Status = HttpStatusCode.OK
        };
    }

    public async Task<CustomResponse<string>> UpdateAllVideosSince(DateTime start)
    {
        var stop = DateTime.Now;

        while (start < stop)
        {
            var from = start;
            var to = start.AddDays(0.5);
            if (to > stop)
                to = stop;

            await UpdateFromToVideos(from, to);

            start = start.AddDays(0.5);
        }

        return new CustomResponse<string>
        {
            Status = HttpStatusCode.OK
        };
    }

    private async Task UpdateFromToVideos(DateTime from, DateTime to)
    {
        var videosDtos = await _updateServant.GetFromToVideos(from, to);

        //Take added/updated videos from last update until now, try to add translation and update the db
        var initialVideosChunk = videosDtos.Content.Select(VideoMapper.FromDto).ToList().ChunkBy(50);
        foreach (var chunk in initialVideosChunk)
        {
            var videos = await UpdateChunk(chunk);
            foreach (var video in videos.Where(video => video.ExternalId != Guid.Empty))
            {
                video.Id = await _videoRepository.GetInternalIdBasedOnExternalId(video.ExternalId);
                video.ModifiedAt = DateTime.Now;
                await _videoRepository.UpdateAsync(video);
            }
        }

        var updateLog = new Update
        {
            FromDate = from,
            ToDate = to,
            Created = DateTime.Now
        };
        await _updateRepository.AddAsync(updateLog);
    }


    //Take valid videos without translation and try to add translation. If add translation failed increase retryCount
    private async Task UpdateDbVideos()
    {
        var validVideosWithoutTranscriptionChunks = (await _videoRepository.GetUntranscriptionedValidVideos()).ChunkBy(50);
        foreach (var chunk in validVideosWithoutTranscriptionChunks)
        {
            var result = await UpdateChunk(chunk);
            foreach (var video in result)
            {
                video.ModifiedAt = DateTime.Now;
                await _videoRepository.UpdateAsync(video);
            }
        }
    }

    private async Task<List<Video>> UpdateChunk(IEnumerable<Video> videosToUpdate)
    {
        var tasksToAddTranscription = videosToUpdate.Select(_updateServant.AddTranscription);
        var videos = (await Task.WhenAll(tasksToAddTranscription)).ToList();
        return videos;
    }
}