namespace TwoFactorAuthentication.API
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using Entities;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public AuthContext() : base("AuthContext") {}
        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(16)]
        public string Psk { get; set; }
    }
}