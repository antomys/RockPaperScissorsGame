using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data.Context;
using Server.Data.Entities;

namespace Server.Data.Extensions;

public static class SeedingExtension
{
    private const string DefaultName = "bot";
    
    public static async Task EnsureBotCreated(this ServerContext context)
    {
        if (await context.Accounts.ContainsAsync(new Account {Id = DefaultName}))
        {
           return;
        }
        
        context.Add(new Account
        {
            Id = DefaultName,
            Login = DefaultName,
            Password = Guid.NewGuid().ToString()
        });
        
        context.Add(new Statistics
        {
            Id = DefaultName,
            AccountId = DefaultName
        });
           
        await context.SaveChangesAsync();
    }
}