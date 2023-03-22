using System.Collections.Generic;

namespace RecSysApi.Domain.Dtos.FeedbackDtos;

public sealed class GetFeedbacksByQueryIdResponseDto
{
    public List<FeedbackDto> FeedbackDtos;
}