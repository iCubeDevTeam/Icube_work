using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.AuthorizationRequirements
{
    public class CustomRequireClaim : IAuthorizationRequirement
    {
        public CustomRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }
        public string ClaimType{get;}

    }
    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaim>
        {
            private readonly DataContext _dbcontext;

            public CustomRequireClaimHandler(DataContext dbcontext)
            {
                _dbcontext = dbcontext;
            }
            protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context,
                CustomRequireClaim requirement)
            {
                var hasFactory = context.User.Claims.Any(x =>x.Type=="FactoryId");
                if (hasFactory)
                {
                    var Factory_id = Int32.Parse(context.User.Claims.FirstOrDefault(x =>x.Type=="FactoryId").Value);
                    var username = context.User.Claims.FirstOrDefault(x =>x.Type==ClaimTypes.NameIdentifier).Value;
                    var user = _dbcontext.Users.Where(x=>x.FactoryId==Factory_id).Include(x=>x.Role).FirstOrDefault(x=>x.Username==username);
                    var has_policy = Convert.ToBoolean(user.Role.GetType().GetProperty(requirement.ClaimType).GetValue(user.Role, null));
                    if (has_policy){
                        context.Succeed(requirement);
                    }
                }
                return Task.CompletedTask;
            }
        }
    
}