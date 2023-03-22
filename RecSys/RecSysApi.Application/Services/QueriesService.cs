using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RecSysApi.Application.Interfaces;
using RecSysApi.Application.Mappers;
using RecSysApi.Domain.Dtos.QueryDtos;
using RecSysApi.Domain.Entities;
using RecSysApi.Domain.Interfaces.Repositories;

namespace RecSysApi.Application.Services;

public sealed class QueriesService : IQueriesService
{
    private readonly IQueryRepository _queryRepository;

    public QueriesService(IQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<CustomResponse<GetQueryByIdResponseDto>> GetQueryById(
        GetQueryByIdDto getQueryByIdDto)
    {
        var query = await _queryRepository.GetByIdAsync(getQueryByIdDto.Id);

        if (query is null)
            return new CustomResponse<GetQueryByIdResponseDto>
            {
                Status = HttpStatusCode.NotFound
            };
        var getQueryByIdResponseDto = new GetQueryByIdResponseDto
        {
            Query = QueryMapper.ToDto(query)
        };

        return new CustomResponse<GetQueryByIdResponseDto>
        {
            Id = Guid.NewGuid(),
            Status = HttpStatusCode.OK,
            Content = getQueryByIdResponseDto
        };
    }

    public async Task<CustomResponse<GetQueriesForUserIdResponseDto>> GetQueriesForUserId(
        GetQueriesForUserIdDto getQueriesForUserIdDto)
    {
        var queries = await _queryRepository.GetQueriesForUserId(getQueriesForUserIdDto.UserId);

        if (!queries.Any())
            return new CustomResponse<GetQueriesForUserIdResponseDto>
            {
                Status = HttpStatusCode.NoContent
            };

        var queriesDto = queries.Select(QueryMapper.ToDto).ToList();
        var getQueriesForUserIdResponseDto = new GetQueriesForUserIdResponseDto
        {
            QueriesDtos = queriesDto
        };

        return new CustomResponse<GetQueriesForUserIdResponseDto>
        {
            Id = Guid.NewGuid(),
            Status = HttpStatusCode.OK,
            Content = getQueriesForUserIdResponseDto
        };
    }

    public async Task<CustomResponse<GetAllQueriesResponseDto>> GetAllQueries()
    {
        var queries = await _queryRepository.GetAllAsync();
        var queriesDto = queries.Select(QueryMapper.ToDto).ToList();
        var getAllQueriesResponseDto = new GetAllQueriesResponseDto
        {
            QueriesDtos = queriesDto
        };
        return new CustomResponse<GetAllQueriesResponseDto>
        {
            Id = Guid.NewGuid(),
            Status = HttpStatusCode.OK,
            Content = getAllQueriesResponseDto
        };
    }
}