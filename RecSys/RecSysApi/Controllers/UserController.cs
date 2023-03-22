using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecSysApi.Application.Interfaces;
using RecSysApi.Domain.Dtos;
using RecSysApi.Domain.Entities.WebApi.Entities;
using RecSysApi.Presentation.Authorization;

namespace RecSysApi.Presentation.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Authorize(Role = Role.Admin)]
    [Route("api/[controller]/addUser")]
    public async Task<IActionResult> AddUser([FromBody] AddUserDto addUserDto)
    {
        var result = await _userService.AddUser(addUserDto);

        return StatusCode((int) result.Status, JsonConvert.SerializeObject(result));
    }

    [HttpPost]
    [Authorize(Role = Role.Admin)]
    [Route("api/[controller]/deleteUser")]
    public async Task<IActionResult> AddUser([FromBody] DeleteUserDto deleteUserDto)
    {
        var result = await _userService.DeleteUser(deleteUserDto);

        return StatusCode((int) result.Status, JsonConvert.SerializeObject(result));
    }
}