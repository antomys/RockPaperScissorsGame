using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Server.Authentication.Models.Interfaces;

namespace Server.Authentication.Models;

/// <summary>
///     Application user 
/// </summary>
internal sealed class ApplicationUser: IApplicationUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="httpContextAccessor"><see cref="IHttpContextAccessor"/>.</param>
    public ApplicationUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <summary>
    ///     This user id
    /// </summary>
    public int Id => GetUserId();
        
    private int GetUserId()
    {
        var request = _httpContextAccessor.HttpContext
            ?.User.Claims.FirstOrDefault(x => x.Type == "id");

        return int.TryParse(request?.Value, out var id) ? id : 0;
    }
}