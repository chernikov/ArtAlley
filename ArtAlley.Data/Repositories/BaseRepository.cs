using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Data.Repositories
{
    public class BaseRepository
    {

        private readonly Lazy<IArtAlleyDbContext> lazyContext;

        private IArtAlleyDbContext context => lazyContext.Value;

        protected readonly Func<IArtAlleyDbContext> getDbContext;

        protected DbContext dbContext => lazyContext.Value as DbContext;

        public BaseRepository(Func<IArtAlleyDbContext> getDbContext)
        {
            this.getDbContext = getDbContext;
            lazyContext = new Lazy<IArtAlleyDbContext>(() => getDbContext());
        }

        protected T Execute<T>(Func<IArtAlleyDbContext, T> functor)
        {
            return functor(context);
        }

        protected void Execute(Action<IArtAlleyDbContext> functor)
        {
            functor(context);
        }

        protected T ExecuteDbContext<T>(Func<DbContext, T> functor)
        {
            return functor(dbContext);
        }

        protected void ExecuteDbContext(Action<DbContext> functor)
        {
            functor(dbContext);
        }
    }
}
