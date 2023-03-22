using System;
using System.ComponentModel.DataAnnotations;

namespace RecSysApi.Domain.Dtos.FeedbackDtos;

public sealed class AddFeedbackDto
{
    [Required]
    public Guid QueryId { get; set; }

    [Required]
    public Guid VideoId { get; set; }

    [Required]
    public decimal TimeSpent { get; set; }
}