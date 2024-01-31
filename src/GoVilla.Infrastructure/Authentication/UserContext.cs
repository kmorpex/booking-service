using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using GoVilla.Application.Abstractions.Authentication;

namespace GoVilla.Infrastructure.Authentication;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string IdentityId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .Claims
            .SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?
            .Value ??
        throw new ApplicationException("User context is unavailable");
}