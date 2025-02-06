namespace BinanceHistoryLoader.Api.Models;

public class HistoricalLoadData
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public List<string> Pairs { get; set; } = null!;
}