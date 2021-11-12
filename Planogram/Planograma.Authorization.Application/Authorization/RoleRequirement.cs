using Microsoft.AspNetCore.Authorization;
using Planograma.Authorization.Application.Services;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Planograma.Authorization.Application.Authorization
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement()
        {
           
        }
        public RoleRequirement(string permission)
        {
            Permission = permission;
        }

        public string Permission { get; }
    }

    public class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IUserService _context;
        

        public RoleHandler(IUserService context )
        {

            _context = context;
            
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,RoleRequirement requirement)
        {
            /*var resourceString = (context.Resource.ToString().Split(' '));
            var getControllerWithAction = resourceString[0].Split('.');
            //var getRoleName = getControllerWithAction[2].Replace("Controller","") + "/" + getControllerWithAction[3];
            /*var getRoleName = ;
            var validRole = context.User.IsInRole(context.Requirements);
            
            if (validRole != true)
            {
                throw new AuthenticationException("Unauthorized valid");
                //context.Fail();
            }*/

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
