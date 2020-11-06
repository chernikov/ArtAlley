using ArtAlley.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Data
{
    public class ArtAlleyDbContext : DbContext, IArtAlleyDbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<TopicFile> TopicFiles { get; set; }

        public ArtAlleyDbContext(DbContextOptions options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    options.UseSqlServer("Server=(local);Initial Catalog=artAlley;Trusted_Connection=True;MultipleActiveResultSets=true");
        //    base.OnConfiguring(options);
        //}
    }
}
