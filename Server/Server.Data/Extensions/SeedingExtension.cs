using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data.Context;
using Server.Data.Entities;

namespace Server.Data.Extensions;

public static class SeedingExtension
{
    public static readonly Player BotPlayer = new()
    {
        Id = Guid.NewGuid().ToString(),
        AccountId = BotId,
        IsReady = true
    };
    
    public const string BotId = "bot";
    
    public static async Task EnsureBotCreated(this ServerContext context)
    {
        var bot = await context.Accounts.FindAsync(BotId);
        if (bot is not null)
        {
           return;
        }

        var botAccount = new Account
        {
            Id = BotId,
            Login = BotId,
            Password = Guid.NewGuid().ToString()
        };
        
        context.Add(botAccount);

        BotPlayer.Account = botAccount;
        
        context.Add(new Statistics
        {
            Id = BotId,
            Account = botAccount,
            AccountId = BotId
        });

        context.Add(BotPlayer);
           
        await context.SaveChangesAsync();
    }
}