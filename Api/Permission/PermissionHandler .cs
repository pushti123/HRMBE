using Helper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Api.Permission
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly CommonHelper _commonHelper;

        public PermissionHandler(CommonHelper commonHelper)
        {
            _commonHelper = commonHelper;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var roleClaim = context.User.FindFirst("RoleId");
            if (roleClaim == null)
                return;

            int roleId = int.Parse(roleClaim.Value);

            foreach (var permission in requirement.Permissions)
            {
                if ( _commonHelper.HasPermission(permission, roleId))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
    }
}