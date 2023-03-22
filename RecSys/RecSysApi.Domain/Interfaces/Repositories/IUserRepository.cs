using System.Threading.Tasks;
using RecSysApi.Domain.Models;

namespace RecSysApi.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserByUsername(string username);
}