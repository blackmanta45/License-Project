using System.Collections.Generic;

namespace RecSysApi.Domain.Dtos.QueryDtos;

public sealed class GetQueriesForUserIdResponseDto
{
    public List<QueryDto> QueriesDtos;
}