using Microsoft.EntityFrameworkCore;

namespace WebApi.Repository
{
    public abstract class RepositoryAbstract : IDisposable
    {
        public DbContext context;

        public RepositoryAbstract(DbContext context)
        {
            this.context = context;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
