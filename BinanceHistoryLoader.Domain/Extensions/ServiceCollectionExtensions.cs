using BinanceHistoryLoader.Domain.Abstractions;
using BinanceHistoryLoader.Domain.Entities;
using BinanceHistoryLoader.Domain.Models;
using BinanceHistoryLoader.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BinanceHistoryLoader.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services, BinanceClientServiceOptions options)
    {
        services.AddSingleton<BinanceClientServiceOptions>(provider => options);
        services.AddHttpClient<BinanceClientService>();
        services.AddScoped<IBinanceClientService, BinanceClientService>();
        services.AddScoped<IJobsRepository, JobsRepository>();
        services.AddScoped<ITradeListsRepository, TradeListsRepository>();
        services.AddScoped<IBinanceClientService, BinanceClientService>();
        services.AddScoped<IBinanceHistoricalDataService, BinanceHistoricalDataService>();

        return services;
    }

    public static IServiceCollection AddMongoClient(this IServiceCollection services, string connection)
    {
        services.AddSingleton<MongoClient>(provider => new MongoClient(connection));

        return services;
    }

    public static IServiceCollection AddMongoDatabase(this IServiceCollection services, string databaseName)
    {
        services.AddSingleton<IMongoDatabase>(provider =>
        {
            var client = provider.GetRequiredService<MongoClient>();

            return client.GetDatabase(databaseName);
        });

        return services;
    }

    public static IServiceCollection AddMongoCollections(this IServiceCollection services,
        string jobCollectionName = "jobs", string tradeListsCollectionName = "tradeLists")
    {
        services.AddSingleton<IMongoCollection<Job>>(provider =>
        {
            var database = provider.GetRequiredService<IMongoDatabase>();

            return database.GetCollection<Job>(jobCollectionName);
        });

        services.AddSingleton<IMongoCollection<AggregateTradeList>>(provider =>
        {
            var database = provider.GetRequiredService<IMongoDatabase>();

            return database.GetCollection<AggregateTradeList>(tradeListsCollectionName);
        });

        return services;
    }
}