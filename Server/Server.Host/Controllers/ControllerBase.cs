using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Server.Host.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    protected string UserId => User.Identity?.Name ?? string.Empty;
}