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


        public void UpdateFiles(int id, List<TopicFile> topicFiles)
            => Execute(context =>
            {
                var existItems = context.TopicFiles.Where(p => p.TopicId == id).ToList();

                var changedItems = existItems.Where(p => topicFiles.Any(tf => tf.Id == p.Id)).ToList();
                var forDelete = existItems.Where(p => !topicFiles.Any(tf => tf.Id == p.Id)).ToList();
                var newItems = topicFiles.Where(p => p.Id == 0);
                context.TopicFiles.RemoveRange(forDelete);
                
                foreach(var item in newItems)
                {
                    item.TopicId = id;
                    context.TopicFiles.Add(item);
                }

                foreach(var item in changedItems)
                {
                    var incomeItem = topicFiles.FirstOrDefault(p => p.Id == item.Id);
                    item.Name = incomeItem.Name;
                }
                context.SaveChanges();
            });
    }
}
