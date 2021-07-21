using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Dal.Context;
using Server.Dal.Entities;

namespace Server.Extensions
{
    public static class SeedingExtension
    {
        public static async Task EnsureBotCreated(this ServerContext context)
        {
            var bot = await context.Accounts.FirstOrDefaultAsync(x => x.Login.ToLower() == "bot");
            if (bot == null)
                await context.AddAsync(new Account
                {
                    Id = 0,
                    Login = "BOT",
                    Password = "SKJSDKBNDFB21321412UIWHFDKSJGNKSDJGN"
                });
            await context.SaveChangesAsync();
        }
    }
}