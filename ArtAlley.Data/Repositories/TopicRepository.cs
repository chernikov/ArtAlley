using ArtAlley.Data.Entities;
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
    }
}
