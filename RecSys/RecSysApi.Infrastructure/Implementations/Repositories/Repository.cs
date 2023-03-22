using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Infrastructure.Context;

namespace RecSysApi.Infrastructure.Implementations.Repositories;

public class Repository<T> : IRepository<T> where T : class, new()
{
    protected readonly UpvDbContext DbContext;

    public Repository(UpvDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IQueryable<T> GetQuery(Expression<Func<T, bool>> expression)
    {
        return DbContext.Set<T>().Where(expression);
    }

    public T Add(T itemToAdd)
    {
        var entity = DbContext.Add(itemToAdd);
        DbContext.SaveChanges();
        return entity.Entity;
    }

    public async Task<T> AddAsync(T itemToAdd)
    {
        var entity = await DbContext.AddAsync(itemToAdd);
        await DbContext.SaveChangesAsync();
        return entity.Entity;
    }

    public T GetById(Guid id)
    {
        return DbContext.Set<T>()
            .Find(id);
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await DbContext.Set<T>()
            .FindAsync(id);
    }

    public T GetById(int id)
    {
        return DbContext.Set<T>()
            .Find(id);
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await DbContext.Set<T>()
            .FindAsync(id);
    }

    public T GetById(string id)
    {
        return DbContext.Set<T>()
            .Find(id);
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await DbContext.Set<T>()
            .FindAsync(id);
    }

    public bool Delete(T itemToDelete)
    {
        DbContext.Remove(itemToDelete);
        DbContext.SaveChanges();
        return true;
    }

    public async Task<bool> DeleteAsync(T itemToDelete)
    {
        DbContext.Remove(itemToDelete);
        await DbContext.SaveChangesAsync();
        return true;
    }

    public IEnumerable<T> GetAll()
    {
        return DbContext.Set<T>()
            .AsEnumerable();
    }

    public int GetCount()
    {
        return DbContext.Set<T>().Count();
    }

    public T Update(T itemToUpdate)
    {
        var entity = DbContext.Update(itemToUpdate);
        DbContext.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<T> UpdateAsync(T itemToUpdate)
    {
        DbContext.ChangeTracker.Clear();
        var entity = DbContext.Update(itemToUpdate);
        await DbContext.SaveChangesAsync();
        return entity.Entity;
    }

    public void Save()
    {
        DbContext.SaveChanges();
    }

    public async Task SaveAsync()
    {
        await DbContext.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await DbContext.Set<T>()
            .ToListAsync();
    }

    public IQueryable<T> GetTable()
    {
        return DbContext.Set<T>();
    }
}