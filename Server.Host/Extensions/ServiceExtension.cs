using Microsoft.Extensions.DependencyInjection;
using Server.Bll.Extensions;

namespace Server.Host.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection service)
    {
        service.AddBusinessLogic();
        
        return service;
    }
}