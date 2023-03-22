using Newtonsoft.Json;
using RecSysApi.Domain.Dtos.QueryDtos;
using RecSysApi.Domain.Dtos.SearchForQueryDtos;
using RecSysApi.Domain.Models;

namespace RecSysApi.Application.Mappers;

public static class QueryMapper
{
    public static QueryDto ToDto(Query query)
    {
        return new QueryDto
        {
            Id = query.Id,
            UserId = query.UserId,
            Search = query.Search,
            Page = query.Page,
            BatchSize = query.BatchSize,
            Result = JsonConvert.DeserializeObject<SearchEngineQueryPaginatedResponseDto>(query.Result)
                     ?? new SearchEngineQueryPaginatedResponseDto(),
            Created = query.Created
        };
    }

    public static Query FromDto(QueryDto queryDto)
    {
        return new Query
        {
            Id = queryDto.Id,
            UserId = queryDto.UserId,
            Search = queryDto.Search,
            Page = queryDto.Page,
            BatchSize = queryDto.BatchSize,
            Result = JsonConvert.SerializeObject(queryDto.Result),
            Created = queryDto.Created
        };
    }
}