using ArtAlley.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Data.Repositories
{
    public class TopicRepository : GenericRepository<Topic>, ITopicRepository
    {
        public TopicRepository(Func<IArtAlleyDbContext> getDbContext) : base(getDbContext)
        {
        }

        public override Topic FindById(int id)
            => Execute(context => context.Topics
                                .Include(p => p.TopicFiles)
                                .FirstOrDefault(p => p.Id == id));

        public Topic FindByUrl(string url)
             => Execute(context => context.Topics
                                .Include(p => p.TopicFiles)
                                .FirstOrDefault(p => p.Url == url));
    }
}
