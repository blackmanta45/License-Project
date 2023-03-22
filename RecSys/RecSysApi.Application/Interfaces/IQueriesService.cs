using System.Threading.Tasks;
using RecSysApi.Domain.Dtos.QueryDtos;
using RecSysApi.Domain.Entities;

namespace RecSysApi.Application.Interfaces;

public interface IQueriesService
{
    Task<CustomResponse<GetQueryByIdResponseDto>> GetQueryById(
        GetQueryByIdDto getQueryByIdDto);

    Task<CustomResponse<GetQueriesForUserIdResponseDto>> GetQueriesForUserId(
        GetQueriesForUserIdDto getQueriesForUserIdDto);

    Task<CustomResponse<GetAllQueriesResponseDto>> GetAllQueries();
}