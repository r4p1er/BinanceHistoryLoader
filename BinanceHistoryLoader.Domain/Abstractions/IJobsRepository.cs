using BinanceHistoryLoader.Domain.Entities;

namespace BinanceHistoryLoader.Domain.Abstractions;

public interface IJobsRepository
{
    Task AddAsync(Job job);

    Task ReplaceByIdAsync(string id, Job job);

    Task<Job?> FindByIdAsync(string id);
}