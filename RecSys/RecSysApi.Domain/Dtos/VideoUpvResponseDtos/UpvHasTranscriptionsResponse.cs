namespace RecSysApi.Domain.Dtos.VideoUpvResponseDtos;

public sealed class UpvHasTranscriptionsResponse
{
    public string Lang { get; set; }
    public string Label { get; set; }
    public string Transcription { get; set; }
}