using SimpleStoreAPI.Interfaces;

namespace SimpleStoreAPI.Service;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;
    }
}
