using IdentityServer.Data;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityServer.IdentityServer
{
    public class ProfileService : IProfileService
    {
        private readonly AuthorizationDbContext dbContext;

        public ProfileService(AuthorizationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string sub = context.Subject.Identity.GetSubjectId();
            var userClaims = await dbContext.UserClaims.Where(x => x.UserId == sub).Select(x => new Claim(x.ClaimType, x.ClaimValue, typeof(string).Name)).ToListAsync();
            var roleClaims = await dbContext.RoleClaims.Where(c => dbContext.UserRoles.Where(x => x.UserId == sub).Select(x => x.RoleId).Contains(c.RoleId)).Select(x => new Claim(x.ClaimType, x.ClaimValue, typeof(string).Name)).ToListAsync();
            List<Claim> claims = new();
            claims.AddRange(userClaims);
            claims.AddRange(roleClaims);

            context.IssuedClaims.AddRange(claims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}