using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UserCrudWebApi.Model;

namespace UserCrudWebApi.Context
{
    public class UserContext : DbContext
    {
            public UserContext(DbContextOptions<UserContext> options) : base(options) { }
            public DbSet<Users> Users { get; set; }
       
    }
}
