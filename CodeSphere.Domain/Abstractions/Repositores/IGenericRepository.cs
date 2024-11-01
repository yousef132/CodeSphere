using CodeSphere.Domain.Premitives;
using Microsoft.EntityFrameworkCore.Storage;

namespace CodeSphere.Domain.Abstractions.Repositores
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

        Task DeleteRangeAsync(ICollection<T> entities);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(Guid id);

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
