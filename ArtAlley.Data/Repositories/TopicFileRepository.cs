using ArtAlley.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Data.Repositories
{
    public class TopicFileRepository : GenericRepository<TopicFile>, ITopicFileRepository
    {
        public TopicFileRepository(Func<IArtAlleyDbContext> getDbContext) : base(getDbContext)
        {
        }

        public void Increase(int id)
            => Execute(context =>
            {
                var exists = context.TopicFiles.FirstOrDefault(p => p.Id == id);
                exists.CountOfPlaying++;
                context.SaveChanges();
            });

        public void UpdateFiles(int id, List<TopicFile> topicFiles)
            => Execute(context =>
            {
                var existItems = context.TopicFiles.Where(p => p.TopicId == id).ToList();

                var changedItems = existItems.Where(p => topicFiles.Any(tf => tf.Id == p.Id)).ToList();
                var forDelete = existItems.Where(p => !topicFiles.Any(tf => tf.Id == p.Id)).ToList();
                var newItems = topicFiles.Where(p => p.Id == 0);
                foreach (var item in forDelete)
                {
                    base.Remove(item);
                }

                foreach (var item in newItems)
                {
                    item.TopicId = id;
                    base.Create(item);
                }

                foreach (var item in changedItems)
                {
                    var incomeItem = topicFiles.FirstOrDefault(p => p.Id == item.Id);
                    item.Name = incomeItem.Name;
                    base.Update(item);
                }
            });
    }
}
