#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecSysApi.Domain.Models;

[Table("queries")]
public class Query
{
    [Key] [Required] public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string? Search { get; set; }
    public decimal? Page { get; set; }
    public decimal? BatchSize { get; set; }
    public string? Result { get; set; }
    public DateTime Created { get; set; }
}