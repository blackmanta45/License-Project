using System.ComponentModel.DataAnnotations;

namespace RecSysApi.Domain.Entities;

public sealed class UserLogin
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}