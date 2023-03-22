using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecSysApi.Domain.Models;

namespace RecSysApi.Domain.Interfaces.Repositories;

public interface IVideoRepository : IRepository<Video>
{
    Task<List<Video>> GetVideosWithIds(List<Guid> videosIds);
    Task<List<Guid>> GetExternalIdsBasedOnInternalIds(List<Guid> videosIds);
    Task<Guid> GetInternalIdBasedOnExternalId(Guid id);
    Task<Video> GetByExternalId(Guid id);
    Task<List<Video>> GetUntranscriptionedValidVideos();
    Task<List<Video>> GetValidVideosWithoutKeywords();
}