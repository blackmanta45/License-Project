using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Domain.Models;
using RecSysApi.Infrastructure.Context;

namespace RecSysApi.Infrastructure.Implementations.Repositories;

public class UpdateRepository : Repository<Update>, IUpdateRepository
{
    private readonly ILogger<UpdateRepository> _logger;

    public UpdateRepository(UpvDbContext upvDbContext, ILogger<UpdateRepository> logger) : base(
        upvDbContext)
    {
        _logger = logger;
    }

    public UpdateRepository(UpvDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<DateTime> GetLastUpdateTime()
    {
        var currentDate = DateTime.Now;
        var lastUpdate = await GetTable().OrderByDescending(p => p.ToDate)
            .FirstOrDefaultAsync();
        return lastUpdate?.ToDate ?? currentDate;
    }
}