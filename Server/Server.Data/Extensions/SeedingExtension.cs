using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data.Context;
using Server.Data.Entities;

namespace Server.Data.Extensions;

public static class SeedingExtension
{
    public const string BotId = "bot";
    
    public static async Task EnsureBotCreated(this ServerContext context)
    {
        if (await context.Accounts.ContainsAsync(new Account {Id = BotId}))
        {
           return;
        }
        
        context.Add(new Account
        {
            Id = BotId,
            Login = BotId,
            Password = Guid.NewGuid().ToString()
        });
        
        context.Add(new Statistics
        {
            Id = BotId,
            AccountId = BotId
        });
           
        await context.SaveChangesAsync();
    }
}