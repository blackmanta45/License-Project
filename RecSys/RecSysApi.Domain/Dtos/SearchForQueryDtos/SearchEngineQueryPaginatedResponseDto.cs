using System.Collections.Generic;

namespace RecSysApi.Domain.Dtos.SearchForQueryDtos
{
    public class SearchEngineQueryPaginatedResponseDto
    {
        public int TotalVideosForQuery { get; set; }
        public IList<SearchEngineQueryVideoPaginatedResponseDto> Videos { get; set; }
    }
}