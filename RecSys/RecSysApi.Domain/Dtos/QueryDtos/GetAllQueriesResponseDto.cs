using System.Collections.Generic;

namespace RecSysApi.Domain.Dtos.QueryDtos;

public sealed class GetAllQueriesResponseDto
{
    public List<QueryDto> QueriesDtos;
}