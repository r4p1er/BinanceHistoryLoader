using System.Net.Http.Json;
using System.Text.Json;
using BinanceHistoryLoader.Domain.Abstractions;
using BinanceHistoryLoader.Domain.Models;

namespace BinanceHistoryLoader.Domain.Services;

public class BinanceClientService : IBinanceClientService
{
    private readonly HttpClient _httpClient;

    public BinanceClientService(HttpClient httpClient, BinanceClientServiceOptions options)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(options.BaseAddress);
    }

    public async Task<List<AggregateTrade>> GetAggTradesAsync(string symbol, DateTime? startTime = null,
        DateTime? endTime = null)
    {
        var endpoint = $"/api/v3/aggTrades?symbol={symbol}";
        if (startTime.HasValue)
            endpoint += $"&startTime={new DateTimeOffset(startTime.Value).ToUnixTimeMilliseconds()}";
        if (endTime.HasValue) endpoint += $"&endTime={new DateTimeOffset(endTime.Value).ToUnixTimeMilliseconds()}";

        return await MakeRequestAsync<List<AggregateTrade>>(endpoint);
    }

    private async Task<T> MakeRequestAsync<T>(string endpoint) where T : new()
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = null
        });

        if (result == null) throw new ArgumentNullException(nameof(T));

        return result;
    }
}