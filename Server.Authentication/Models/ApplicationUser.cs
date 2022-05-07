using System;
using Microsoft.AspNetCore.Http;
using Server.Authentication.Models.Interfaces;

namespace Server.Authentication.Models;

/// <summary>
///     Application user.
/// </summary>
internal sealed class ApplicationUser: IApplicationUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="httpContextAccessor"><see cref="IHttpContextAccessor"/>.</param>
    public ApplicationUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <summary>
    ///     Retrieved user id from JWT token.
    /// </summary>
    public string Id => GetUserId();
        
    private string GetUserId()
    {
        var request = _httpContextAccessor.HttpContext
            ?.User.Identity?.Name;

        return request ?? throw new ArgumentNullException(nameof(request));
    }
}