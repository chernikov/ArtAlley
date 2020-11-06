using ArtAlley.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Data.Entities.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(Func<IArtAlleyDbContext> getDbContext) : base(getDbContext)
        {
        }

        public User Login(User user)
            => Execute(context =>
            {
                var entity = context.Users.FirstOrDefault(p => p.Login == user.Login && p.Password == user.Password);
                return entity;
            });
        
    }
}
