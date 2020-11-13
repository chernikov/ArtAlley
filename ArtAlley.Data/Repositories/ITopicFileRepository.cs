using ArtAlley.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Data.Repositories
{
    public interface ITopicFileRepository : IGenericRepository<TopicFile>, IBaseRepository
    {
        void UpdateFiles(int id, List<TopicFile> topicFiles);
        void Increase(int id);
    }
}
