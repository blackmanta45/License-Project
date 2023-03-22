using RecSysApi.Domain.Dtos.QueryDtos;
using RecSysApi.Domain.Dtos.SearchForQueryDtos;
using RecSysApi.Domain.Dtos.VideosQueryDtos;

namespace RecSysApi.Application.Interfaces.VideosLookup;

public interface IVideosLookupServant
{
    QueryDto CreateQueryDto(GetVideosQueryPaginatedDto getVideosQueryPaginatedDto,
        SearchEngineQueryPaginatedResponseDto searchEngineQueryPaginatedResponseDto);
}