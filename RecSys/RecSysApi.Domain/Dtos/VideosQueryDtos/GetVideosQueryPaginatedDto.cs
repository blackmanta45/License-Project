namespace RecSysApi.Domain.Dtos.VideosQueryDtos;

public class GetVideosQueryPaginatedDto
{
    public string UserId { get; set; }
    public string Query { get; set; }
    public string Page { get; set; }
    public string ChunkSize { get; set; }
}