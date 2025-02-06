using BinanceHistoryLoader.Domain.Models;
using BinanceHistoryLoader.Domain.Services;

namespace Testt;

class Program
{
    static async Task Main(string[] args)
    {
        
        var httpClient = new HttpClient();
        var options = new BinanceClientServiceOptions
        {
            BaseAddress = "https://api.binance.com"
        };
        var client = new BinanceClientService(httpClient, options);
        var result = await client.GetAggTradesAsync("LTCBTC");
        Console.WriteLine(result);
        
    }
}
