using BinanceHistoryLoader.Domain.Models;

namespace BinanceHistoryLoader.Domain.Abstractions;

public interface IBinanceClientService
{
    Task<List<AggregateTrade>> GetAggTradesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken cancellationToken = default);
}