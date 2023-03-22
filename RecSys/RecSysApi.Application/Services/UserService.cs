using System;
using System.Net;
using System.Threading.Tasks;
using RecSysApi.Application.Interfaces;
using RecSysApi.Domain.Dtos;
using RecSysApi.Domain.Entities;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Domain.Models;

namespace RecSysApi.Application.Services;

public class UserService : IUserService
{
    private readonly ILoginService _loginService;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository,
        ILoginService loginService)
    {
        _userRepository = userRepository;
        _loginService = loginService;
    }

    public async Task<CustomResponse<string>> AddUser(AddUserDto user)
    {
        var userDb = await _userRepository.GetUserByUsername(user.Username);
        if (userDb is not null)
            return new CustomResponse<string>
            {
                Status = HttpStatusCode.Conflict,
                Content = "Username already exists"
            };

        var passwordHash = _loginService.CalculateHash(user.Password);
        await _userRepository.AddAsync(new User
        {
            Username = user.Username,
            Hash = passwordHash,
            Created = DateTime.Now
        });

        return new CustomResponse<string>
        {
            Status = HttpStatusCode.OK
        };
    }

    public async Task<CustomResponse<string>> DeleteUser(DeleteUserDto user)
    {
        var dbUser = await _userRepository.GetUserByUsername(user.Username);
        if (dbUser is null)
            return new CustomResponse<string>
            {
                Status = HttpStatusCode.NotFound,
                Content = "User doesn't exist"
            };

        await _userRepository.DeleteAsync(dbUser);

        return new CustomResponse<string>
        {
            Status = HttpStatusCode.OK
        };
    }
}