using System;
using System.ComponentModel.DataAnnotations;

namespace RecSysApi.Domain.Dtos.FeedbackDtos;

public sealed class GetFeedbacksByVideoIdDto
{
    [Required]
    public Guid VideoId { get; set; }
}