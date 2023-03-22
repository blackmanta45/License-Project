using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Domain.Models;
using RecSysApi.Infrastructure.Context;

namespace RecSysApi.Infrastructure.Implementations.Repositories;

public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
{
    private readonly ILogger<FeedbackRepository> _logger;

    public FeedbackRepository(UpvDbContext upvDbContext, ILogger<FeedbackRepository> logger) : base(
        upvDbContext)
    {
        _logger = logger;
    }

    public FeedbackRepository(UpvDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Feedback>> GetFeedbacksByQueryId(Guid queryId)
    {
        return await GetQuery(e => e.Query.Id == queryId)
            .Include(x => x.Query)
            .Include(x => x.Video)
            .ToListAsync();
    }

    public async Task<List<Feedback>> GetFeedbacksByVideoId(Guid videoId)
    {
        return await GetQuery(e => e.Video.Id == videoId)
            .Include(x => x.Query)
            .Include(x => x.Video)
            .ToListAsync();
    }
}