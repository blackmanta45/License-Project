using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecSysApi.Domain.Dtos.VideoUpvResponseDtos;
using RecSysApi.Domain.Entities;
using RecSysApi.Domain.Models;

namespace RecSysApi.Application.Interfaces.Update;

public interface IUpdateServant
{
    Task<Video> AddTranscription(Video video);
    Task<CustomResponse<List<VideoUpvResponseDto>>> GetFromToVideos(DateTime fromDate, DateTime toDate);
    Task<CustomResponse<string>> ComputeKeywords(string transcript);
}