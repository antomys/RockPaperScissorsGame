using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Dal.Context;
using Server.Dal.Entities;

namespace Server.Dal.Extensions;

public static class SeedingExtension
{
    private const string DefaultName = "bot";
    
    public static async Task EnsureBotCreated(this ServerContext context)
    {
        var bot = await context.Accounts.FirstOrDefaultAsync(account => account.Login.Equals(DefaultName, StringComparison.OrdinalIgnoreCase));
        if (bot == null)
            await context.AddAsync(new Account
            {
                Id = 0,
                Login = DefaultName,
                Password = "SKJSDKBNDFB21321412UIWHFDKSJGNKSDJGN"
            });
           
        await context.SaveChangesAsync();
    }
}