#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecSysApi.Domain.Models;

[Table("updates")]
public class Update
{
    [Key] [Required] public Guid Id { get; set; }

    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public DateTime Created { get; set; }
}