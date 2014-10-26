namespace TwoFactorAuthentication.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Services;

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

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            IdentityResult result = await userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            IdentityResult result = await userManager.AddLoginAsync(userId, login);

            return result;
        }

        public async Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo)
        {
            ApplicationUser user = await userManager.FindAsync(loginInfo);

            return user;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return ctx.RefreshTokens.ToList();
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            ctx.RefreshTokens.Remove(refreshToken);
            return await ctx.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            RefreshToken refreshToken = await ctx.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            RefreshToken refreshToken = await ctx.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                ctx.RefreshTokens.Remove(refreshToken);
                return await ctx.SaveChangesAsync() > 0;
            }

            return false;
        }

        public Client FindClient(string clientId)
        {
            Client client = ctx.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            RefreshToken existingToken =
                ctx.RefreshTokens.SingleOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

            if (existingToken != null)
            {
                bool result = await RemoveRefreshToken(existingToken);
            }

            ctx.RefreshTokens.Add(token);

            return await ctx.SaveChangesAsync() > 0;
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