using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManager.Models;

namespace UserManager.Data
{
    public class UserDbContext : IdentityDbContext<UserDetails>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
    }
}
