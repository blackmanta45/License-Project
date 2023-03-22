using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecSysApi.Application.Mappers;
using RecSysApi.Domain.Dtos.QueryDtos;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Domain.Models;
using RecSysApi.Infrastructure.Context;

namespace RecSysApi.Infrastructure.Implementations.Repositories;

public class QueryRepository : Repository<Query>, IQueryRepository
{
    private readonly ILogger<QueryRepository> _logger;

    public QueryRepository(UpvDbContext dbContext, ILogger<QueryRepository> logger) : base(dbContext)
    {
        _logger = logger;
    }

    public QueryRepository(UpvDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Query>> GetQueriesForUserId(Guid userId)
    {
        return await GetQuery(e => e.UserId == userId).ToListAsync();
    }

    public async Task<Query> AddQuery(QueryDto queryDto)
    {
        var query = QueryMapper.FromDto(queryDto);
        var queryDb = await AddAsync(query);
        return queryDb;
    }
}