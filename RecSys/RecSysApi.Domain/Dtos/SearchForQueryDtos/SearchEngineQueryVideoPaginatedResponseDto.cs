using System;
using Newtonsoft.Json;
using RecSysApi.Domain.Commons;

namespace RecSysApi.Domain.Dtos.SearchForQueryDtos
{
    public class SearchEngineQueryVideoPaginatedResponseDto
    {
        [JsonConverter(typeof(GuidConverter))]
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}