using BinanceHistoryLoader.Domain.Entities;

namespace BinanceHistoryLoader.Domain.Abstractions;

public interface IBinanceHistoricalDataService
{
    string LoadData(List<string> pairs, DateTime startDate, DateTime endDate);

    Task<Job> CheckStatus(string id);
}