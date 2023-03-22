using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecSysApi.Domain.Models;

[Table("feedback")]
public sealed class Feedback
{
    [Key] [Required] public Guid Id { get; set; }
    [Required] public Query Query { get; set; }
    [Required] public Video Video { get; set; }
    public decimal TimeSpent { get; set; }
    public DateTime Created { get; set; }
}