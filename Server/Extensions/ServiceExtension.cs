using System;
using Microsoft.Extensions.DependencyInjection;
using Server.Bll.Services;
using Server.Bll.Services.Interfaces;


namespace Server.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
           return service.
               AddSingleton<IAccountService, AccountService>();
        }
    }
}