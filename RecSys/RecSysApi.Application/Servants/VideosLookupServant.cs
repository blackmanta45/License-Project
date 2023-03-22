using System;
using RecSysApi.Application.Interfaces.VideosLookup;
using RecSysApi.Domain.Dtos.QueryDtos;
using RecSysApi.Domain.Dtos.SearchForQueryDtos;
using RecSysApi.Domain.Dtos.VideosQueryDtos;

namespace RecSysApi.Application.Servants;

public sealed class VideosLookupServant : IVideosLookupServant
{
    public QueryDto CreateQueryDto(GetVideosQueryPaginatedDto getVideosQueryPaginatedDto,
        SearchEngineQueryPaginatedResponseDto searchEngineQueryPaginatedResponseDto)
    {
        Guid userId;
        try
        {
            userId = Guid.Parse(getVideosQueryPaginatedDto.UserId);
        }
        catch
        {
            userId = Guid.Empty;
        }

        return new QueryDto
        {
            UserId = userId,
            Search = getVideosQueryPaginatedDto.Query,
            Page = Convert.ToDecimal(getVideosQueryPaginatedDto.Page),
            BatchSize = Convert.ToDecimal(getVideosQueryPaginatedDto.ChunkSize),
            Result = searchEngineQueryPaginatedResponseDto,
            Created = DateTime.Now
        };
    }
}