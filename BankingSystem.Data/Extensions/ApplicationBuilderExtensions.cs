using BankingSystem.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSystem.Data.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task ApplyBankingMigrationsAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
        await context.Database.MigrateAsync();
    }
}
