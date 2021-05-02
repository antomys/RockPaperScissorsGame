using Microsoft.Extensions.DependencyInjection;
using Server.GameLogic.LogicServices;
using Server.GameLogic.LogicServices.Impl;
using Server.Services;
using Server.Services.Interfaces;

namespace Server.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
           return service.AddSingleton(typeof(IDeserializedObject<>), typeof(DeserializedObject<>))
            .AddTransient(typeof(IStorage<>), typeof(Storage<>))
            .AddSingleton<IAccountManager, AccountManager>()
            .AddSingleton<IRoundCoordinator, RoundCoordinator>()
            .AddSingleton<IRoomCoordinator, RoomCoordinator>();
        }
    }
}