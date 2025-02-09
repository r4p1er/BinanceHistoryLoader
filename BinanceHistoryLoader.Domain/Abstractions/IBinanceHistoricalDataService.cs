using BinanceHistoryLoader.Domain.Entities;

namespace BinanceHistoryLoader.Domain.Abstractions;

public interface IBinanceHistoricalDataService
{
    string LoadData(List<string> pairs, DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken = default);

    Task<Job> CheckStatusAsync(string id);
}