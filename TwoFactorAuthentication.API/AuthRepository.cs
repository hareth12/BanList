using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TwoFactorAuthentication.API.Models;
using TwoFactorAuthentication.API.Services;

namespace TwoFactorAuthentication.API
{
    public class AuthRepository : IDisposable
    {
        private readonly AuthContext ctx;

        private readonly UserManager<ApplicationUser> userManager;

        public AuthRepository()
        {
            ctx = new AuthContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
        }

        public void Dispose()
        {
            ctx.Dispose();
            userManager.Dispose();
        }

        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            var user = new ApplicationUser
            {
                UserName = userModel.UserName,
                TwoFactorEnabled = true,
                Psk = TimeSensitivePassCode.GeneratePresharedKey()
            };

            IdentityResult result = await userManager.CreateAsync(user, userModel.Password);

            return result;
        }
    }
}