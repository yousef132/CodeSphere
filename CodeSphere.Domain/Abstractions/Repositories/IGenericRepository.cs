﻿using CodeSphere.Domain.Premitives;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace CodeSphere.Domain.Abstractions.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

        Task DeleteRangeAsync(ICollection<T> entities);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);

        Task SaveChangesAsync();

        IDbContextTransaction BeginTransaction();

        IQueryable<T> GetTableAsNotTracked();
        IQueryable<T> GetTableAsTracked();

        Task AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);

        Task UpdateRangeAsync(ICollection<T> entities);
        void Commit();
        void RollBack();

    }
}