using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionHandler(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return; // 沒有 userId，授權失敗
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://permissions-service/api/permissions/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return; // API 查詢失敗，授權失敗
            }

            var json = await response.Content.ReadAsStringAsync();
            var permissions = JsonSerializer.Deserialize<List<string>>(json);

            if (permissions != null && permissions.Contains(requirement.RequiredPermission))
            {
                context.Succeed(requirement); // 符合條件，授權成功
            }
        }
    }
}
