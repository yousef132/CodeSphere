using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Premitives;
using CodeSphere.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CodeSphere.Infrastructure.Implementation.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        #region Vars / Props
        private readonly ApplicationDbContext context;

        #endregion


        #region Constructor
        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        #endregion


        #region Methods
        public async Task AddAsync(T entity)
        => await context.Set<T>().AddAsync(entity);


        public async Task AddRangeAsync(ICollection<T> entities)
           => await context.Set<T>().AddRangeAsync(entities);

        public IDbContextTransaction BeginTransaction() => context.Database.BeginTransaction();

        public void Commit() => context.Database.CommitTransaction();
        public async Task DeleteAsync(T entity)
           => context.Set<T>().Remove(entity);

        public async Task DeleteRangeAsync(ICollection<T> entities)
           => context.Set<T>().RemoveRange(entities);

        public async Task<T> GetByIdAsync(int id) => await context.Set<T>().FindAsync(id);

        public IQueryable<T> GetTableAsTracked() => context.Set<T>().AsQueryable();

        public IQueryable<T> GetTableAsNotTracked() => context.Set<T>().AsNoTracking().AsQueryable();

        public void RollBack() => context.Database.RollbackTransaction();

        public async Task SaveChangesAsync() => await context.SaveChangesAsync();
        public async Task UpdateAsync(T entity)
            => context.Set<T>().Update(entity);
        public async Task UpdateRangeAsync(ICollection<T> entities)
            => context.Set<T>().UpdateRange(entities);

        #endregion
    }
}
