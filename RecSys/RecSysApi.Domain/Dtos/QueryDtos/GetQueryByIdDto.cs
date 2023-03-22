using System;

namespace RecSysApi.Domain.Dtos.QueryDtos;

public sealed class GetQueryByIdDto
{
    public Guid Id { get; set; }
}