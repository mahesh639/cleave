using Microsoft.AspNetCore.Identity;

namespace UserManager.Models
{
    public class UserDetails : IdentityUser
    {
        public string Name { get; set; }
    }
}
