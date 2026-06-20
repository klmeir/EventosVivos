using EventosVivos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosVivos.Infrastructure.Repositories
{
    public class GenericRepository<T> where T : class
    {
        protected readonly EventosVivosDbContext DbContext;
        protected readonly DbSet<T> DbSet;

        public GenericRepository(EventosVivosDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);            
        }

        public async Task UpdateAsync(T entity)
        {
            DbSet.Update(entity);            
        }
    }
}
