using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecSysApi.Domain.Dtos.QueryDtos;
using RecSysApi.Domain.Models;

namespace RecSysApi.Domain.Interfaces.Repositories;

public interface IQueryRepository : IRepository<Query>
{
    Task<List<Query>> GetQueriesForUserId(Guid userId);
    Task<Query> AddQuery(QueryDto queryDto);
}