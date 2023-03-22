using System;
using System.ComponentModel.DataAnnotations;

namespace RecSysApi.Domain.Dtos.FeedbackDtos;

public sealed class GetFeedbacksByQueryIdDto
{
    [Required]
    public Guid QueryId { get; set; }
}