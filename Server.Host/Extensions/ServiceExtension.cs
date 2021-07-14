using Microsoft.Extensions.DependencyInjection;
using Server.Authentication.Models;
using Server.Authentication.Models.Interfaces;
using Server.Bll.HostedServices;
using Server.Bll.Services;
using Server.Bll.Services.Interfaces;

namespace Server.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddTransient<IApplicationUser, ApplicationUser>()
                .AddHostedService<CleanerHostedService>();
            service.AddHttpContextAccessor();

            // In this way I am registering multiple interfaces to one Transient instance of RoomService;
            service
                .AddTransient<RoomService>()
                .AddTransient<IRoomService>(provider => provider.GetRequiredService<RoomService>())
                .AddTransient<IHostedRoomService>(provider => provider.GetRequiredService<RoomService>());

            return service;

        }
    }
}