using BankingSystem.Data.Data;
using BankingSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSystem.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBankingData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("BankingConnection")
            ?? "Data Source=../bankingsystem.db";

        services.AddDbContext<BankingDbContext>(options =>
            options.UseSqlite(connectionString, sqlite => sqlite.MigrationsAssembly(typeof(BankingDbContext).Assembly.FullName)));
        services.AddScoped<IBankRepository, BankRepository>();

        return services;
    }
}
