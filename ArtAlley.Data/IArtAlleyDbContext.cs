using ArtAlley.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ArtAlley.Data
{
    public interface IArtAlleyDbContext
    {
         DbSet<User> Users { get; set; }

         DbSet<Topic> Topics { get; set; }

         DbSet<TopicFile> TopicFiles { get; set; }

        int SaveChanges();
    }
}
