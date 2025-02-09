using BinanceHistoryLoader.Domain.Entities;

namespace BinanceHistoryLoader.Domain.Abstractions;

public interface IJobsRepository
{
    Task AddAsync(Job job, CancellationToken cancellationToken = default);

    Task ReplaceByIdAsync(string id, Job job, CancellationToken cancellationToken = default);

    Task<Job?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
}