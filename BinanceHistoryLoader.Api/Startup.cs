using BinanceHistoryLoader.Domain.Extensions;
using BinanceHistoryLoader.Domain.Models;

namespace BinanceHistoryLoader.Api;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection collection)
    {
        collection.AddControllers();

        collection.AddEndpointsApiExplorer();
        collection.AddSwaggerGen();

        var options = new BinanceClientServiceOptions
        {
            BaseAddress = configuration["BinanceApi:BaseAddress"]!
        };
        collection.AddDomain(options);
        collection.AddMongoClient(configuration["Mongo:Connection"]!);
        collection.AddMongoDatabase(configuration["Mongo:DatabaseName"]!);
        collection.AddMongoCollections(configuration["Mongo:JobsName"]!, configuration["Mongo:TradeListsName"]!);
        collection.AddLogging(builder => builder.AddConsole());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}