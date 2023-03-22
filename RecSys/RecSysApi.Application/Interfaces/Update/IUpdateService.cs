using System;
using System.Threading.Tasks;
using RecSysApi.Domain.Entities;

namespace RecSysApi.Application.Interfaces.Update;

public interface IUpdateService
{
    Task<CustomResponse<string>> UpdateVideos();
    Task<CustomResponse<string>> UpdateAllVideosSince(DateTime start);
}