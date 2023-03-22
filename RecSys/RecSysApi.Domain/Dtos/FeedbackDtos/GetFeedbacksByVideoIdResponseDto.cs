using System.Collections.Generic;

namespace RecSysApi.Domain.Dtos.FeedbackDtos;

public sealed class GetFeedbacksByVideoIdResponseDto
{
    public List<FeedbackDto> FeedbackDtos;
}