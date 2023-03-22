using System.Threading.Tasks;
using RecSysApi.Domain.Dtos.VideosQueryDtos;
using RecSysApi.Domain.Entities;

namespace RecSysApi.Application.Interfaces.VideosLookup;

public interface IVideosLookupService
{
    Task<CustomResponse<GetVideosQueryPaginatedResponseDto>> LookupForVideos(
        GetVideosQueryPaginatedDto getVideosQueryPaginatedDto);
}