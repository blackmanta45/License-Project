using System.Threading.Tasks;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Domain.Interfaces.UnitOfWork;
using RecSysApi.Infrastructure.Context;
using RecSysApi.Infrastructure.Implementations.Repositories;

namespace RecSysApi.Infrastructure.Implementations.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IUserRepository _userRepository;

        public UnitOfWork(UpvDbContext dbContext)
        {
            _videoRepository = new VideoRepository(dbContext);
            _queryRepository = new QueryRepository(dbContext);
            _userRepository = new UserRepository(dbContext);
        }

        public async Task Save()
        {
            await _videoRepository.SaveAsync();
            await _userRepository.SaveAsync();
            await _queryRepository.SaveAsync();
        }
    }
}