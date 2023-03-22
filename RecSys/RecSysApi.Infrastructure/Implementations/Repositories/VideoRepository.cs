using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Domain.Models;
using RecSysApi.Infrastructure.Context;

namespace RecSysApi.Infrastructure.Implementations.Repositories;

public class VideoRepository : Repository<Video>, IVideoRepository
{
    private readonly ILogger<VideoRepository> _logger;

    public VideoRepository(UpvDbContext upvDbContext, ILogger<VideoRepository> logger) : base(
        upvDbContext)
    {
        _logger = logger;
    }

    public VideoRepository(UpvDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Video>> GetVideosWithIds(List<Guid> videosIds)
    {
        return await GetQuery(e => videosIds.Any(r => r == e.Id)).ToListAsync();
    }

    public async Task<List<Guid>> GetExternalIdsBasedOnInternalIds(List<Guid> videosIds)
    {
        return await GetTable().Where(e => videosIds.Any(r => r == e.Id)).Select(x => x.ExternalId).ToListAsync();
    }

    public async Task<Guid> GetInternalIdBasedOnExternalId(Guid id)
    {
        return await GetTable().Where(e => e.ExternalId == id).Select(x => x.Id).FirstOrDefaultAsync();
    }

    public async Task<Video> GetByExternalId(Guid id)
    {
        return await GetTable().Where(e => e.ExternalId == id).FirstOrDefaultAsync();
    }

    public async Task<List<Video>> GetUntranscriptionedValidVideos()
    {
        return await GetQuery(e =>
                e.UpdateDate > DateTime.Now.AddDays(-3) && e.HasTranscription == false && e.DeletionDate == null)
            .ToListAsync();
    }

    public async Task<List<Video>> GetValidVideosWithoutKeywords()
    {
        return await GetQuery(e => e.HasTranscription == true && e.DeletionDate == null && e.Keywords == null)
            .Where(x => !string.IsNullOrEmpty(x.Transcription)).ToListAsync();
    }
}