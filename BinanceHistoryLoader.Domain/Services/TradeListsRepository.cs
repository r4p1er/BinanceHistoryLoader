using BinanceHistoryLoader.Domain.Abstractions;
using BinanceHistoryLoader.Domain.Entities;
using MongoDB.Driver;

namespace BinanceHistoryLoader.Domain.Services;

public class TradeListsRepository(IMongoCollection<AggregateTradeList> tradeLists) : ITradeListsRepository
{
    public async Task AddAsync(AggregateTradeList tradeList, CancellationToken cancellationToken = default)
    {
        await tradeLists.InsertOneAsync(tradeList, null, cancellationToken);
    }
}