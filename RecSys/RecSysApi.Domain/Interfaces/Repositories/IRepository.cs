using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RecSysApi.Domain.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        Task<List<T>> GetAllAsync();
        int GetCount();
        IQueryable<T> GetQuery(Expression<Func<T, bool>> expression);
        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        T GetById(string id);
        Task<T> GetByIdAsync(string id);
        T Add(T itemToAdd);
        Task<T> AddAsync(T itemToAdd);
        T Update(T itemToUpdate);
        Task<T> UpdateAsync(T itemToUpdate);
        bool Delete(T itemToDelete);
        Task<bool> DeleteAsync(T itemToDelete);
        void Save();
        Task SaveAsync();
    }
}
