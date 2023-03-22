#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecSysApi.Domain.Models;

[Table("videosUPV")]
public class Video
{
    [Key] [Required] public Guid Id { get; set; }

    [Required] public Guid ExternalId { get; set; }

    public string? Title { get; set; }
    public string? Transcription { get; set; }
    public string? Keywords { get; set; }
    public bool? HasTranscription { get; set; }
    public bool? Hidden { get; set; }
    public DateTime? CreationDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeletionDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}