using System;
using RecSysApi.Domain.Dtos.SearchForQueryDtos;

namespace RecSysApi.Domain.Dtos.QueryDtos;

public sealed class QueryDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Search { get; set; }
    public decimal? Page { get; set; }
    public decimal? BatchSize { get; set; }
    public SearchEngineQueryPaginatedResponseDto Result { get; set; }
    public DateTime Created { get; set; }
}