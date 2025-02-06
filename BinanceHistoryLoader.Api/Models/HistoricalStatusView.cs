using BinanceHistoryLoader.Domain.Entities;
using BinanceHistoryLoader.Domain.Enums;

namespace BinanceHistoryLoader.Api.Models;

public class HistoricalStatusView
{
    public string JobId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? EndTime { get; set; }

    public static HistoricalStatusView FromJob(Job job)
    {
        var result = new HistoricalStatusView
        {
            JobId = job.Id.ToString(),
            EndTime = job.EndTime
        };

        switch (job.Status)
        {
            case JobStatus.Processing: result.Status = "В обработке"; break;
            case JobStatus.Completed: result.Status = "Завершено"; break;
            case JobStatus.Failed: result.Status = "Ошибка"; break;
        }

        return result;
    }
}