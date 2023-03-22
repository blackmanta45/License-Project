using System;
using System.Threading.Tasks;
using RecSysApi.Domain.Models;

namespace RecSysApi.Domain.Interfaces.Repositories;

public interface IUpdateRepository : IRepository<Update>
{
    Task<DateTime> GetLastUpdateTime();
}