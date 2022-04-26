using System.Linq;
using Microsoft.AspNetCore.Http;
using Server.Authentication.Models.Interfaces;

namespace Server.Authentication.Models;

/// <summary>
/// Application user 
/// </summary>
internal sealed class AuthUser: IAuthUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="httpContextAccessor">http accessor</param>
    public AuthUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// This user шв
    /// </summary>
    public int Id => GetUserId();
        
    private int GetUserId()
    {
        var request = _httpContextAccessor.HttpContext
            ?.User.Claims.FirstOrDefault(x => x.Type == "id");

        return int.TryParse(request?.Value, out var id) ? id : 0;
    }
}