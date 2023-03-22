using System.Threading.Tasks;
using RecSysApi.Domain.Dtos.SearchForQueryDtos;
using RecSysApi.Domain.Dtos.VideosQueryDtos;
using RecSysApi.Domain.Entities;

namespace RecSysApi.Domain.Interfaces.Services;

public interface ISearchEngineService
{
    Task<CustomResponse<SearchEngineQueryPaginatedResponseDto>> SendQueryToSfq(
        GetVideosQueryPaginatedDto getVideosQueryPaginatedDtoString);
}