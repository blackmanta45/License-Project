using System.ComponentModel.DataAnnotations;

namespace RecSysApi.Domain.Dtos;

public sealed class DeleteUserDto
{
    [Required]
    public string Username { get; set; }
}