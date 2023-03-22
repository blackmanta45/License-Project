using System.Threading.Tasks;
using RecSysApi.Domain.Dtos;
using RecSysApi.Domain.Entities;

namespace RecSysApi.Application.Interfaces;

public interface IUserService
{
    Task<CustomResponse<string>> AddUser(AddUserDto user);
    Task<CustomResponse<string>> DeleteUser(DeleteUserDto user);
}