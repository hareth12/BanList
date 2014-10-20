using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TwoFactorAuthentication.API
{
    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public AuthContext() : base("AuthContext") {}
    }

    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(16)]
        public string Psk { get; set; }
    }
}