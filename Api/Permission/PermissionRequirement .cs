using Microsoft.AspNetCore.Authorization;

namespace Api.Permission
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public List<string> Permissions { get; }

        public PermissionRequirement(List<string> permissions)
        {
            Permissions = permissions;
        }
    }

}
