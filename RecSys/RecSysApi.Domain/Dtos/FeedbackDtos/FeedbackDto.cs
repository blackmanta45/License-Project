using System;

namespace RecSysApi.Domain.Dtos.FeedbackDtos;

public sealed class FeedbackDto
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public Guid QueryId { get; set; }
    public decimal TimeSpent { get; set; }
    public DateTime Created { get; set; }
}