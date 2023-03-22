using System;
using RecSysApi.Domain.Dtos.FeedbackDtos;
using RecSysApi.Domain.Models;

namespace RecSysApi.Application.Mappers;

public static class FeedbackMapper
{
    public static FeedbackDto ToDto(Feedback feedback)
    {
        return new FeedbackDto
        {
            Id = feedback.Id,
            VideoId = feedback.Video.Id,
            QueryId = feedback.Query.Id,
            TimeSpent = feedback.TimeSpent,
            Created = feedback.Created
        };
    }

    public static Feedback FromDto(AddFeedbackDto feedbackDto, Query query, Video video)
    {
        return new Feedback
        {
            Query = query,
            Video = video,
            TimeSpent = feedbackDto.TimeSpent,
            Created = DateTime.Now
        };
    }
}