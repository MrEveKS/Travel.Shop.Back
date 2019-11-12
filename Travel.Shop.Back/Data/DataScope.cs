using System;
using System.Threading;
using System.Threading.Tasks;

namespace Travel.Shop.Back.Data
{
    public class DataScope : IDisposable
    {
        public BaseDbContext DbContext { get; }

        public DataScope(BaseDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
