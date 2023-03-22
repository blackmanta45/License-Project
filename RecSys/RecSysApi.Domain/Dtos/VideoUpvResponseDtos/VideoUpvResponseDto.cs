using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RecSysApi.Domain.Commons;

namespace RecSysApi.Domain.Dtos.VideoUpvResponseDtos
{
    public class VideoUpvResponseDto
    {
        [JsonProperty("_id")]
        [JsonConverter(typeof(GuidConverter))]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<string> Keywords { get; set; }
        public string Language { get; set; }

        [JsonConverter(typeof(GuidConverter))]
        public Guid? Owner { get; set; }
        public List<string> UnescoCodes { get; set; }
        public bool? Hidden { get; set; }

        [JsonProperty("transcription_ingested_date")]
        public DateTime? TranscriptionIngestedDate { get; set; }   
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeletionDate { get; set; }
    }
}
