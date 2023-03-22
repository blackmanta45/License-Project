using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Domain.Models;
using RecSysApi.Infrastructure.Context;

namespace RecSysApi.Infrastructure.Implementations.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(UpvDbContext upvDbContext, ILogger<UserRepository> logger) : base(
        upvDbContext)
    {
        _logger = logger;
    }

    public UserRepository(UpvDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await GetQuery(e => e.Username == username).FirstOrDefaultAsync();
    }
}