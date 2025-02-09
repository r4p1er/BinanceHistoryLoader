using BinanceHistoryLoader.Domain.Entities;

namespace BinanceHistoryLoader.Domain.Abstractions;

public interface ITradeListsRepository
{
    Task AddAsync(AggregateTradeList tradeList, CancellationToken cancellationToken = default);
}