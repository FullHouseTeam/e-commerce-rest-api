using Microsoft.EntityFrameworkCore;
using rest_web_api.Models;

namespace rest_web_api.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
        : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
