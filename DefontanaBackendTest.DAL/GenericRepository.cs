using DefontanaBackendTest.DL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DefontanaBackendTest.DAL
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private TestDBContext context;
        private DbSet<TEntity> dbSet;

        public GenericRepository(TestDBContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await dbSet.AsNoTracking().ToListAsync();
    }
}
