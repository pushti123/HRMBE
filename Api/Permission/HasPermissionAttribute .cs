using Microsoft.AspNetCore.Authorization;

namespace Api.Permission
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        private const string POLICY_PREFIX = "Permission:";

        public HasPermissionAttribute(params string[] permissions)
        {
            Policy = POLICY_PREFIX + string.Join(",", permissions);
        }
    }


}
