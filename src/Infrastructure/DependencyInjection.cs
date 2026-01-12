using Application.Abstractions;
using Infrastructure.Http;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BlockchainDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IBlockchainSnapshotRepository, BlockchainSnapshotRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHttpClient("blockcypher")
            .AddPolicyHandler(GetRetryPolicy());

        services.AddScoped<IBlockchainClientService, BlockchainClientService>();

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(retry));
}