using System.ComponentModel.DataAnnotations;

namespace RecSysApi.Domain.Dtos;

public sealed class AddUserDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}