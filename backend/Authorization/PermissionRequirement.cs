using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string RequiredPermission { get; }

        public PermissionRequirement(string requiredPermission)
        {
            RequiredPermission = requiredPermission;
        }
    }
}
