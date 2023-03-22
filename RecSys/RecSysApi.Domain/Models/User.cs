using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecSysApi.Domain.Models;

[Table("users")]
public sealed class User
{
    [Key] [Required] public Guid Id { get; set; }
    public string Username { get; set; }
    public string Hash { get; set; }
    public string Role { get; set; }
    public DateTime Created { get; set; }
}