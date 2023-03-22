using System;

namespace RecSysApi.Domain.Dtos.QueryDtos;

public sealed class GetQueriesForUserIdDto
{
    public Guid UserId { get; set; }
}