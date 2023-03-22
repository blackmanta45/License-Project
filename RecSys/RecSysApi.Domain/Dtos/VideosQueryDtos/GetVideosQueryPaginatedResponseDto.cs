using System;
using System.Collections.Generic;

namespace RecSysApi.Domain.Dtos.VideosQueryDtos
{
    public class GetVideosQueryPaginatedResponseDto
    {
        public Guid QueryId { get; set; }
        public int TotalVideosForQuery { get; set; }
        public IList<Guid> VideosIds { get; set; }
    }
}