using Microsoft.AspNetCore.Identity;

namespace UserRoles.Identity.Models
{
    public class Users : IdentityUser
    {
        public string? Fullname { get; set; }
    }
}
