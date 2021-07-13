using Microsoft.Extensions.DependencyInjection;
using Server.Authentication.Models;
using Server.Authentication.Models.Interfaces;
using Server.Bll.Services;
using Server.Bll.Services.Interfaces;

namespace Server.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddTransient<IApplicationUser, ApplicationUser>();
            service.AddHttpContextAccessor();
            
            return service
                .AddTransient<IRoomService, RoomService>();
        }
    }
}