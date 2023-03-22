using System;
using RecSysApi.Domain.Dtos.VideoUpvResponseDtos;
using RecSysApi.Domain.Models;

namespace RecSysApi.Application.Mappers;

public static class VideoMapper
{
    public static Video FromDto(VideoUpvResponseDto videoUpvResponseDto)
    {
        return new Video
        {
            ExternalId = videoUpvResponseDto.Id,
            Title = videoUpvResponseDto.Title,
            Transcription = null,
            Keywords = null,
            HasTranscription = false,
            Hidden = videoUpvResponseDto.Hidden,
            CreationDate = videoUpvResponseDto.CreationDate,
            UpdateDate = videoUpvResponseDto.UpdateDate,
            DeletionDate = videoUpvResponseDto.DeletionDate,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
    }
}