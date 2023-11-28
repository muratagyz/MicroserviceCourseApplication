using Microsoft.AspNetCore.Http;

namespace Course.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId { get => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value; set => throw new System.NotImplementedException(); }
    }
}